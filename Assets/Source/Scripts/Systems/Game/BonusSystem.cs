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
        
        [SerializeField] private Transform container;
        
        public override void OnInit()
        {
            base.OnInit();

            filter = world.Filter<HoverableComponent>().Inc<MergeTypeComponent>().End();
            
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
            }
        }

        private void BonusOnClick(BonusView bonusView)
        {
            if (save.Money - bonusView.Cost < 0)
            {
                return;
            }
            
            save.Money -= bonusView.Cost;
            pool.UpdateMoneyEvent.Add(eventWorld.NewEntity());
            
            Debug.Log(bonusView.BonusType);
            
            switch (bonusView.BonusType)
            {
                case BonusType.AutoMerge:
                    StartCoroutine(AutoMerge());
                    break;
                case BonusType.Shake:
                    BonusShake();
                    break;
            }
        }

        private void BonusShake()
        {
            // platform.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, shakeStrength), vibrato, randomness: 90, snapping: false, fadeOut: true);

            for (int i = container.childCount - 1; i >= 0; i--)
            {
                var child = container.GetChild(i);
                child.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, shakeStrength), vibrato,
                    randomness: 90, snapping: false, fadeOut: true);
            }
        }

        private IEnumerator AutoMerge()
        {
            pool.MergeEvent.Add(eventWorld.NewEntity());
            
            yield return new WaitForEndOfFrame();
            
            int i = 0;
            foreach (var e in filter)
            {
                if (i >= 2)
                {
                    break;
                }

                pool.Hoverable.Get(e).IsHovered = true;

                i++;

            }
            pool.MergeEvent.Add(eventWorld.NewEntity());
            
        }
    }
    
    
}