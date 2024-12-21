using System.Collections.Generic;
using DG.Tweening;
using Kuhpik;
using Source.Scripts.View;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Screens
{
    public class BonusUIScreen : UIScreen
    {
        
        public List<BonusView> BonusViews = new List<BonusView>();
        [Header("Reward Panel")]
        public Button RewardButton;
        public Button CloseButton;
        
        [Header("Animation Panel")]
        public GameObject RewardPanel;
        
        public CanvasGroup CanvasGroup; 
        public float windowShowDuration = 2f; 
        
        public void ShowPanel()
        {
            Sequence sequence = DOTween.Sequence();
            RewardPanel.transform.localScale = Vector3.zero;
            
            sequence.Append(CanvasGroup.DOFade(1f, windowShowDuration)).OnStart(() => RewardPanel.SetActive(true))
                .Join(RewardPanel.transform.DOScale(Vector3.one, windowShowDuration).SetEase(Ease.OutBack)); 
        }

        public void HidePanel()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(CanvasGroup.DOFade(0f, windowShowDuration))  
                .Join(RewardPanel.transform.DOScale(Vector3.zero, windowShowDuration).SetEase(Ease.OutBack)) 
                .OnKill(() => RewardPanel.SetActive(false));
        }
        
    }
}