using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.Components.Events;
using Source.Scripts.Components.View;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class LevelSystem : GameSystem
    {
        [SerializeField] private Vector3 minPosition;
        [SerializeField] private Vector3 maxPosition;
        [SerializeField] private Transform container;
        
        private List<GameObject> _levelObjects = new List<GameObject>();
        private EcsFilter filter;
        private EcsFilter filter2;
        public override void OnInit()
        {
            base.OnInit();
            
            filter = eventWorld.Filter<StartLevelEvent>().End();
            filter2 = world.Filter<HoverableComponent>().Inc<MergeTypeComponent>().Inc<RigidbodyComponent>().End();
            StartLevel();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var e in filter)
            {
                StartLevel();
            }
        }

        public void StartLevel()
        {
            ClearChildren();
            
            foreach (var obj in config.LevelStartMobs[save.CurrentLevel])
            {
                for (int i = 0; i < obj.Value; i++)
                {
                    var rndPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), 
                        Random.Range(minPosition.y, maxPosition.y), 
                        Random.Range(minPosition.z, maxPosition.z));
                    var rndRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                    
                    var newObj = Instantiate(config.MergeObjects[obj.Key], rndPosition, rndRotation, container);
                    
                    _levelObjects.Add(newObj);
                }
            }
            
            foreach (var obj in _levelObjects)
            {
                var view = obj.GetComponent<BaseView>();
                game.Fabric.InitView(view);
            }
            
            _levelObjects.Clear();
        }

        private void ClearChildren()
        {
            foreach (var e in filter2)
            {
                pool.DeadTag.Add(e);
            }
        }
    }
}