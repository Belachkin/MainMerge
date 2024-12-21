using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.Components.View;
using Source.Scripts.Data;
using Source.Scripts.SDK;
using Source.Scripts.UI.Screens;
using Source.Scripts.View;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Scripts.Systems.Game
{
    public class BonusSystem : GameSystemWithScreen<BonusUIScreen>
    {
        
        [SerializeField] private Transform  platform; // Ссылка на платформу
        [SerializeField] private float shakeDuration = 1f; // Длительность шейка
        [SerializeField] private float shakeStrength = 1f; // Сила шейка
        [SerializeField] private int vibrato = 10; // Частота вибрации

        [Header("Bonus Shake")] 
        [SerializeField] private GameObject TNTPrefab;
        [SerializeField] private Vector3 spawnPosition = Vector3.zero;
        [SerializeField] private Material baseMaterial;
        [SerializeField] private Material whiteMaterial;
        [SerializeField] private ParticleSystem explosionParticle;
        [Space(5)]
        
        private Vector3 initialPosition; // Начальная позиция платформы

        private EcsFilter filter;
        private EcsFilter rbFilter;
        private bool isMistake = true;
        
        [SerializeField] private Transform container;
        
        public override void OnInit()
        {
            base.OnInit();
            
            YandexManager.Instance.RewardClaimEvent += HideRewPanel;
            
            game.Tasks.Clear();
            
            filter = world.Filter<HoverableComponent>().Inc<MergeTypeComponent>().End();
            rbFilter = world.Filter<RigidbodyComponent>().End();

             screen.CloseButton.onClick.AddListener(() => {screen.HidePanel(); game.MergeState = MergeStateType.Merge;});
             screen.RewardButton.onClick.AddListener(() => { YandexManager.Instance.ShowRewardedAd(); });
            
            InitBonuses();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        
            foreach (var bonusView in screen.BonusViews)
            {
                if (save.CurrentTutorStepType == TutorStepType.MERGE_1 ||
                    save.CurrentTutorStepType == TutorStepType.MERGE_2 ||
                    save.CurrentTutorStepType == TutorStepType.MERGE_3 ||
                    save.CurrentTutorStepType == TutorStepType.WAIT_BONUS ||
                    (bonusView.BonusType == BonusType.Shake && save.CurrentTutorStepType != TutorStepType.DONE) ||
                    game.MergeState == MergeStateType.MergeLock)
                {
                    bonusView.Button.interactable = false;
                }
                else
                {
                    bonusView.Button.interactable = true;
                }
            }
        }

        private void InitBonuses()
        {
            foreach (var bonusView in screen.BonusViews)
            {
                bonusView.Button.onClick.AddListener(() => BonusOnClick(bonusView));
                bonusView.CostText.text = bonusView.Cost.ToString();
            }
        }

        private void BonusOnClick(BonusView bonusView)
        {
            if (save.Money - bonusView.Cost < 0)
            {
                ShowRewPanel();
                return;
            }
            
            bonusView.Button.transform.DOScale(0.8f, 0.1f).OnComplete(() =>
            {
                bonusView.Button.transform.DOScale(1f, 0.1f);
            });
            
            save.Money -= bonusView.Cost;
            pool.UpdateMoneyEvent.Add(eventWorld.NewEntity());
            
            Debug.Log(bonusView.BonusType);
            
            switch (bonusView.BonusType)
            {
                case BonusType.AutoMerge:
                    StartCoroutine(AutoMerge(bonusView));

                    if (save.CurrentTutorStepType == TutorStepType.BONUS)
                    {
                        save.CurrentTutorStepType = TutorStepType.DONE;
                    }
                    
                    break;
                case BonusType.Shake:
                    StartCoroutine(BonusShake());
                    break;
            }
        }

        private IEnumerator BonusShake()
        {

            var TNT = Instantiate(TNTPrefab, spawnPosition, Quaternion.identity);
            var TNT_Mesh = TNT.GetComponent<MeshRenderer>();
            
            
            pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = audioConfig.TNTActivation;

            float seconds = 0;

            while (seconds < 1.5f)
            {
                TNT_Mesh.material = baseMaterial;
                yield return new WaitForSeconds(0.125f);
                TNT_Mesh.material = whiteMaterial;
                yield return new WaitForSeconds(0.125f);
                seconds += 0.25f;
            }
            
            // yield return new WaitForSeconds(1.5f);
            
            Vector3 explosionPosition = new Vector3(TNT.transform.position.x, 
                                                    TNT.transform.position.y, 
                                                    TNT.transform.position.z);
            Destroy(TNT);
            
            var particle = Instantiate(explosionParticle.gameObject, explosionPosition, Quaternion.identity);
            
            
            pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = audioConfig.TNTExplosion;
            
            foreach (var e in rbFilter)
            {
                var rb = pool.Rb.Get(e);
                
                Vector3 direction = (rb.Value.position - explosionPosition).normalized;
                
                float impulseMultiplier = 20.0f;
                Vector3 randomOffset = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                );
                
                Vector3 impulse = (direction + randomOffset).normalized * impulseMultiplier;
                
                rb.Value.AddForce(impulse, ForceMode.Impulse);
            }
            yield return new WaitForSeconds(1.5f);
            Destroy(particle);
        }

        private IEnumerator AutoMerge(BonusView bonusView)
        {
            pool.ClearSelectedEvent.Add(eventWorld.NewEntity());
            
            yield return new WaitForEndOfFrame();

            
            
            foreach (var task in game.Tasks)
            {
                if (task.Value > 0)
                {
                    var mergingObjects = new List<int>();

                    MergeObjectType mergeType = task.Key;
                    
                    // if (config.MergeLevels.Count - 1 != config.MergeLevels.IndexOf(task.Key))
                    // {
                    //     mergeType = config.MergeLevels[config.MergeLevels.IndexOf(task.Key) - 1];
                    //     Debug.Log($"Merge Type: {config.MergeLevels[config.MergeLevels.IndexOf(task.Key) - 1]}");
                    // }
                   
                    
                    foreach (var e in filter)
                    {
                        
                        if (pool.MergeType.Get(e).MergeType == mergeType)
                        {
                            mergingObjects.Add(e);
                            
                        }
                        
                    }
                    
                    if (mergingObjects.Count >= 2)
                    {
                        bool completed = false;

                        isMistake = true;
                        
                        for (int i = 0; i < 2; i++)
                        {
                            yield return new WaitForSeconds(0.25f);
                            
                            pool.Hoverable.Get(mergingObjects[i]).IsHovered = true;
                            completed = true;
                        }

                        if (completed == false)
                        {
                            bonusView.Button.interactable = false;
                        
                            bonusView.Button.transform
                                .DOShakeScale(shakeDuration, new Vector3(shakeStrength, shakeStrength, shakeStrength), vibrato, 
                                    randomness: 90, fadeOut: true)
                                .OnComplete(() =>
                                {
                                    save.Money += bonusView.Cost;
                                
                                    pool.UpdateMoneyEvent.Add(eventWorld.NewEntity());
                                
                                    bonusView.Button.interactable = true;
                                });
                        }
                        
                        yield break;
                    }
                    
                    
                }
            }
            
            if(isMistake == true)
            {
                bonusView.Button.interactable = false;
                        
                bonusView.Button.transform
                    .DOShakeScale(shakeDuration, new Vector3(shakeStrength, shakeStrength, shakeStrength), vibrato, 
                        randomness: 90, fadeOut: true)
                    .OnComplete(() =>
                    {
                        save.Money += bonusView.Cost;
                                
                        pool.UpdateMoneyEvent.Add(eventWorld.NewEntity());
                                
                        bonusView.Button.interactable = true;
                    });
                
            }

            if (isMistake == false)
            {
                isMistake = true;
            }
            
        }

        private void ShowRewPanel()
        {
            game.MergeState = MergeStateType.MergeLock;
            screen.ShowPanel();
        }

        private void HideRewPanel()
        {
            screen.HidePanel();
            save.Money += 300;
            
            pool.UpdateMoneyEvent.Add(eventWorld.NewEntity());
            
            game.MergeState = MergeStateType.Merge;
        }
        
    }
    
    
}