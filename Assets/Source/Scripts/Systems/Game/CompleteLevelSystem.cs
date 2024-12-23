using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.Data;
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

                game.MergeState = MergeStateType.MergeLock;
                
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
                
                screen.ThisLevelText.text = save.CurrentLevel.ToString();

                if (save.CurrentLevel == 0 && save.CurrentTutorStepType == TutorStepType.MERGE_1 ||
                    save.CurrentTutorStepType == TutorStepType.MERGE_2)
                {
                    save.CurrentTutorStepType = TutorStepType.WAIT_BONUS_AUTOMERGE;
                }
                
                Debug.Log(save.CurrentLevel);
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

            if (save.CurrentTutorStepType == TutorStepType.WAIT_BONUS_AUTOMERGE)
            {
                save.CurrentTutorStepType = TutorStepType.BONUS_AUTOMERGE;
            }
            else if (save.CurrentTutorStepType == TutorStepType.WAIT_BONUS_TNT)
            {
                save.CurrentTutorStepType = TutorStepType.BONUS_TNT;
            }
            
            game.MergeState = MergeStateType.Merge;
        }
    }
}