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
        public override void OnInit()
        {
            base.OnInit();
            
            filter = eventWorld.Filter<LevelCompleteEvent>().End();
            // screen.NextLevelButton.onClick.AddListener(() => NextLevel());
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
                
                screen.ShowPanel();
            }
        }
        
        
        private void NextLevel()
        {
            screen.HidePanel();
            pool.StartLevelEvent.Add(eventWorld.NewEntity());
        }
    }
}