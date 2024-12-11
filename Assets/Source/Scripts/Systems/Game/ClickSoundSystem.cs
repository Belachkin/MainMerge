using Kuhpik;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Systems.Game
{
    public class ClickSoundSystem : GameSystem
    {
        [SerializeField] private Button[] buttons;
        [SerializeField] private Button[] buttonsBuy;
        [SerializeField] private Button[] buttonsSell;

        public override void OnInit()
        {
            base.OnInit();
            foreach (var button in buttons)
            {
                button.onClick.AddListener((() =>
                {
                    pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = audioConfig.ClickSound;
                }));
            }
            
            foreach (var button in buttonsBuy)
            {
                button.onClick.AddListener((() =>
                {
                    //pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = config.ClickSound;
                    pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = audioConfig.BuySound;
                }));
            }
            
            /* foreach (var button in buttonsSell)
             {
                 button.onClick.AddListener((() =>
                 {
                     //pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = config.ClickSound;
                     pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = config.SellSound;
                 }));
             }*/
        }
    }
}