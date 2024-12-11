using DG.Tweening;
using Kuhpik;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Screens
{
    public class LevelCompleteUIScreen : UIScreen
    {
        public GameObject victoryWindow;
        public ParticleSystem victoryParticles; 
        public CanvasGroup victoryWindowCanvasGroup; 
        public float windowShowDuration = 1f; 
        public float particlesShowDuration = 0.5f; 
        
        public Button NextLevelButton;

        public void ShowPanel()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(victoryWindowCanvasGroup.DOFade(1f, windowShowDuration))  
                .Join(victoryWindow.transform.DOScale(Vector3.one, windowShowDuration)) 
                .OnStart(() => victoryWindow.SetActive(true)); 
            
            sequence.AppendCallback(() => victoryParticles.Play()) 
                .AppendInterval(particlesShowDuration);
        }

        public void HidePanel()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(victoryWindowCanvasGroup.DOFade(0f, windowShowDuration))  
                .Join(victoryWindow.transform.DOScale(Vector3.zero, windowShowDuration)) 
                .OnKill(() => victoryWindow.SetActive(false));
        }
    }
}