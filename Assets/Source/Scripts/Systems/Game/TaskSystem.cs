using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

                if (game.Tasks.ContainsKey(mergeType))
                {
                    game.Tasks[mergeType] -= 1;
                    UpdateTask(mergeType);
                }
            }

            foreach (var e in filter2)
            {
                isComplete = false;
                InitTasks();
            }

            if (isComplete == false)
            {
                int i = 0;
            
                foreach (var task in game.Tasks)
                {
                    if (task.Value <= 0)
                    {
                        i++;
                    }
                }

                if (i >= game.Tasks.Count)
                {
                    isComplete = true;
                    pool.LevelCompleteEvent.Add(eventWorld.NewEntity());
                }
            }
        }

        private void InitTasks()
        {
            var lvlTask = config.LevelTasks[save.CurrentLevel];
            
            InitTasks(lvlTask, config.TaskIcons);
            
        }
        
        public void InitTasks(
            SerializedDictionary<MergeObjectType, int> levelTasks, 
            SerializedDictionary<MergeObjectType, Sprite> taskIcons)
        {

            for (int i = screen.taskPanel.childCount - 1; i >= 0; i--)
            {
                Transform child = screen.taskPanel.GetChild(i);
                Destroy(child.gameObject);
            }
        
            game.Tasks.Clear();
            screen.taskTexts.Clear();
        
            foreach (var task in levelTasks)    
            {
                game.Tasks.Add(task.Key, task.Value);
            
                var newTaskUI = Instantiate(screen.taskUIPrefab, screen.taskPanel);
            
                newTaskUI.GetComponent<Image>().sprite = taskIcons[task.Key];
            
                var newTaskText = newTaskUI.GetComponentInChildren<TextMeshProUGUI>();
                newTaskText.text = task.Value.ToString();
            
                screen.taskTexts.Add(task.Key, newTaskText);
            
            }
        }

        public void UpdateTask(MergeObjectType mergeObjectType)
        {
            GameObject animateObject = null;
            if (game.Tasks[mergeObjectType] <= 0)
            {
                var img = screen.taskTexts[mergeObjectType].gameObject.GetComponentInChildren<Image>();
            
                img.enabled = true;
            
                animateObject = img.gameObject;
                screen.taskTexts[mergeObjectType].enabled = false;
            }
            else
            {
                screen.taskTexts[mergeObjectType].text = game.Tasks[mergeObjectType].ToString();
                animateObject = screen.taskTexts[mergeObjectType].gameObject;
            }
        
            animateObject.transform
                .DOScale(screen.animationScale, screen.animationDuration) 
                .OnComplete(() => 
                    animateObject.transform.DOScale(1f, screen.animationDuration) 
                );
        }
    }
}