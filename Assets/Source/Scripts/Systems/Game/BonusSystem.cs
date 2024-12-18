using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.Components.View;
using Source.Scripts.Data;
using Source.Scripts.UI.Screens;
using Source.Scripts.View;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Systems.Game
{
    public class BonusSystem : GameSystemWithScreen<BonusUIScreen>
    {
        [Header("Bonus Shake")]
        [SerializeField] private Transform  platform; // Ссылка на платформу
        [SerializeField] private float shakeDuration = 1f; // Длительность шейка
        [SerializeField] private float shakeStrength = 1f; // Сила шейка
        [SerializeField] private int vibrato = 10; // Частота вибрации
        
        private Vector3 initialPosition; // Начальная позиция платформы

        private EcsFilter filter;
        private EcsFilter rbFilter;
        private bool isMistake = true;
        
        [SerializeField] private Transform container;
        
        public override void OnInit()
        {
            base.OnInit();
            
            game.Tasks.Clear();
            
            filter = world.Filter<HoverableComponent>().Inc<MergeTypeComponent>().End();
            rbFilter = world.Filter<RigidbodyComponent>().End();
            
            InitBonuses();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var bonusView in screen.BonusViews)
            {
                if (save.Money - bonusView.Cost < 0)
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
                    BonusShake();
                    break;
            }
        }

        private void BonusShake()
        {
            
            foreach (var e in rbFilter)
            {
                var rb = pool.Rb.Get(e);
                
                float impulseMultiplier = 2.0f;
                Vector3 impulse = new Vector3(Random.Range(-10, 10), Random.Range(0.1f, 0.5f), Random.Range(-10, 10)) * impulseMultiplier;
                
                rb.Value.AddForce(impulse, ForceMode.Impulse);
            }
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
    }
    
    
}