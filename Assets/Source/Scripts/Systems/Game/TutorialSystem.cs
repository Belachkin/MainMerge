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
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject finger;
        
        [SerializeField] private float scaleMultiplier = 1.2f; 
        [SerializeField] private float animationDuration = 0.5f;

        [SerializeField] private Vector2 fingerOffset;
        
        private Transform startTransform;
        private Sequence fingerSequence;
        private Camera mainCamera;
        
        private EcsFilter filter;

        
        
        public override void OnInit()
        {
            base.OnInit();

            filter = eventWorld.Filter<FingerSetPositionEvent>().End();
            
            mainCamera = Camera.main;
            
            
            // fingerOffset = new Vector2(finger.transform.position.x + fingerOffset.x, finger.transform.position.y + fingerOffset.y);
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
        
        public void FingerSetPosition(Vector3 targetPosition)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            
            // Если Canvas в режиме Screen Space - Overlay
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                finger.transform.position = screenPosition + (Vector3)fingerOffset; 
            }
            // Если Canvas в режиме Screen Space - Camera или World Space
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    screenPosition,
                    canvas.worldCamera,
                    out Vector2 localPosition
                );

                finger.transform.localPosition = localPosition + fingerOffset;
            }
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