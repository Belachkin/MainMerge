using Kuhpik;
using Source.Scripts.SDK;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class OtherGamesUIScreen : UIScreen
    {
        [SerializeField] private Button otherGamesBtn;
        
        public override void Subscribe()
        {
            base.Subscribe();
            otherGamesBtn.onClick.AddListener(()=>
            {
                string currentDomain = YandexManager.Instance.OtherGamesURL;
                Application.OpenURL(currentDomain);
            });
        }
    }
}