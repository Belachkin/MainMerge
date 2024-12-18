using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.Data;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class TutorialSystem : GameSystem
    {
        [SerializeField] private GameObject finger;
        
        [SerializeField] private float scaleMultiplier = 1.2f; 
        [SerializeField] private float animationDuration = 0.5f; 
        
        private Transform startTransform;
        private Sequence fingerSequence;
        
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();

            filter = eventWorld.Filter<FingerSetPositionEvent>().End();
            
            startTransform = finger.transform;
            
            finger.gameObject.SetActive(false);
            
            if (save.CurrentTutorStepType != TutorStepType.DONE)
            {
                finger.gameObject.SetActive(true);
                FingerAnimation();
            }
            
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var e in filter)
            {
                var newPosition = pool.FingerSetPositionEvent.Get(e).Position;
                
                FingerSetPosition(newPosition);
            }

            if (save.CurrentTutorStepType == TutorStepType.DONE)
            {
                fingerSequence.Kill();
                finger.gameObject.SetActive(false);
            }
        }
        
        public void FingerSetPosition(Vector3 position)
        {
            finger.transform.position = position;
        }
        
        public void FingerAnimation()
        {
            fingerSequence = DOTween.Sequence();
            
            fingerSequence.Append(finger.transform.DOScale(startTransform.localScale * scaleMultiplier, animationDuration)
                    .SetEase(Ease.InOutSine)) 
                .Append(finger.transform.DOScale(startTransform.localScale, animationDuration)
                    .SetEase(Ease.InOutSine)); 
            
            fingerSequence.SetLoops(-1, LoopType.Yoyo);
        }

        
    }
}