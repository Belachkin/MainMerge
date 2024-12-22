using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.UI.Screens;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class FloatingTextSystem : GameSystemWithScreen<FloatingTextUIScreen>
    {
        
        
        private EcsFilter filter;
        public override void OnInit()
        {
            base.OnInit();

            filter = eventWorld.Filter<FloatingTextEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var e in filter)
            {
                var floatingData = pool.FloatingTextEvent.Get(e);

                ShowFloatingText(floatingData);
                
                
            }
        }
        
        public void ShowFloatingText(FloatingTextEvent floatingData)
        {
            var floatingTextObject = screen.FloatingTextObject;
            var floatingText = screen.FloatingText;
            
            floatingTextObject.SetActive(true); 
            // floatingText.text = floatingData.text; 
            //
            // floatingText.color = floatingData.color;
            //
            // // Начальная позиция текста
            // Vector2 startPosition = floatingData.position;
            // Vector2 targetPosition = startPosition + new Vector2(0, 50); // Двигаем вверх на 50
            //
            // floatingTextObject.transform.localPosition = startPosition;
            //
            // // Анимация движения
            // floatingTextObject.transform.DOLocalMove(targetPosition, 1f).SetEase(Ease.OutCubic);
            //
            // // Анимация исчезновения
            // floatingText.DOFade(0, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
            // {
            //     floatingTextObject.SetActive(false); // Отключаем объект после завершения
            //     floatingTextObject.transform.localPosition = startPosition; // Возвращаем позицию
            // });
            
            
            floatingText.text = floatingData.text;
            floatingText.color = floatingData.color;
            RectTransform rectTransform = floatingText.GetComponent<RectTransform>();
            
            rectTransform.position = floatingData.position;
            
            Sequence sequence = DOTween.Sequence();
            
            floatingText.color = new Color(floatingText.color.r, floatingText.color.g, floatingText.color.b, 0);
            sequence.Append(floatingText.DOFade(1, screen.fadeDuration));
            
            sequence.Join(rectTransform.DOMoveY(rectTransform.position.y + screen.moveDistance, screen.lifetime));
            
            sequence.Append(floatingText.DOFade(0, screen.fadeDuration));
            
            sequence.OnComplete(() => {floatingTextObject.SetActive(false);});
        }
    }
}