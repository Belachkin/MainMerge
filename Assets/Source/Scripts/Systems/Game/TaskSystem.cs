using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.Data;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class TaskSystem : GameSystemWithScreen<GameUIScreen>
    {
        private EcsFilter filter;
        private EcsFilter filter2;
        private bool isComplete = false;
        public override void OnInit()
        {
            base.OnInit();

            filter = eventWorld.Filter<MergeEvent>().End();
            filter2 = eventWorld.Filter<StartLevelEvent>().End();
            
            InitTasks();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            foreach (var e in filter)
            {
                ref var mergeType = ref pool.MergeEvent.Get(e).MergeType;

                screen.tasks[mergeType] -= 1;
                screen.UpdateTask(mergeType);
            }

            foreach (var e in filter2)
            {
                isComplete = false;
                InitTasks();
            }

            if (isComplete == false)
            {
                int i = 0;
            
                foreach (var task in screen.tasks)
                {
                    if (task.Value <= 0)
                    {
                        i++;
                    }
                }

                if (i >= screen.tasks.Count)
                {
                    isComplete = true;
                    pool.LevelCompleteEvent.Add(eventWorld.NewEntity());
                }
            }
        }

        private void InitTasks()
        {
            var lvlTask = config.LevelTasks[save.CurrentLevel];
            
            screen.InitTasks(lvlTask, config.TaskIcons);
            
        }

        
    }
}