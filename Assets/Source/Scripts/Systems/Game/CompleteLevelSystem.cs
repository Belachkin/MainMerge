using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.UI.Screens;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class CompleteLevelSystem : GameSystemWithScreen<LevelCompleteUIScreen>
    {
        private EcsFilter filter;
        private EcsFilter filter2;
        public override void OnInit()
        {
            base.OnInit();
            
            filter = eventWorld.Filter<LevelCompleteEvent>().End();
            filter2 = eventWorld.Filter<AddMoneyEvent>().End();
            screen.NextLevelButton.onClick.AddListener(() => NextLevel());
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var e in filter)
            {
                if (save.CurrentLevel < save.MaxLevel)
                {
                    save.CurrentLevel++;
                }
                else
                {
                    save.CurrentLevel = 0;
                }
                
                screen.ThisLevelText.text = save.CurrentLevel.ToString();
                
                pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = audioConfig.VictorySound;
                
                screen.ShowPanel();
            }

            foreach (var e in filter2)
            {
                var money = pool.AddMoneyEvent.Get(e).Value;
                screen.AddMoneyText.text = $"+{money}";
            }
        }
        
        private void NextLevel()
        {
            screen.HidePanel();
            pool.StartLevelEvent.Add(eventWorld.NewEntity());
        }
    }
}