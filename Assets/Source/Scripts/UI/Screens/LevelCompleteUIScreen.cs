using DG.Tweening;
using Kuhpik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Screens
{
    public class LevelCompleteUIScreen : UIScreen
    {
        public GameObject victoryWindow;
        public ParticleSystem victoryParticles; 
        public CanvasGroup victoryWindowCanvasGroup; 
        public float windowShowDuration = 2f; 
        public float particlesShowDuration = 1f; 
        
        public Button NextLevelButton;
        
        public TextMeshProUGUI ThisLevelText;
        public TextMeshProUGUI AddMoneyText;
        
        public void ShowPanel()
        {
            
            Sequence sequence = DOTween.Sequence();
            victoryWindow.transform.localScale = Vector3.zero;
            
            sequence.Append(victoryWindowCanvasGroup.DOFade(1f, windowShowDuration)).OnStart(() => victoryWindow.SetActive(true))
                .Join(victoryWindow.transform.DOScale(Vector3.one, windowShowDuration).SetEase(Ease.OutBack)); 
            
            sequence.AppendCallback(() => victoryParticles.Play()) 
                .AppendInterval(particlesShowDuration);
            
            sequence.AppendInterval(1f);

        }

        public void HidePanel()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(victoryWindowCanvasGroup.DOFade(0f, windowShowDuration))  
                .Join(victoryWindow.transform.DOScale(Vector3.zero, windowShowDuration).SetEase(Ease.OutBack)) 
                .OnKill(() => victoryWindow.SetActive(false));
        }
    }
}