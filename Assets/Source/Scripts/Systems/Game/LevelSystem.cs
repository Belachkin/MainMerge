using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.Components.Events;
using Source.Scripts.Components.View;
using Source.Scripts.Data;
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
            StartCoroutine(SpawnWithDelay());
            // foreach (var obj in config.LevelStartMobs[save.CurrentLevel])
            // {
            //     for (int i = 0; i < obj.Value; i++)
            //     {
            //         Vector3 rndPosition;
            //         int attempts = 0;
            //         do
            //         {
            //             rndPosition = new Vector3(
            //                 Random.Range(minPosition.x, maxPosition.x),
            //                 Random.Range(minPosition.y, maxPosition.y),
            //                 Random.Range(minPosition.z, maxPosition.z)
            //             );
            //             attempts++;
            //         } while (Physics.CheckSphere(rndPosition, 0.5f) && attempts < 10); // Проверяем на столкновения
            //
            //         var rndRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            //         var newObj = Instantiate(config.MergeObjects[obj.Key], rndPosition, rndRotation, container);
            //         _levelObjects.Add(newObj);
            //     }
            // }
            //
            // foreach (var obj in _levelObjects)
            // {
            //     var view = obj.GetComponent<BaseView>();
            //     game.Fabric.InitView(view);
            // }
            //
            // _levelObjects.Clear();
        }
        
        private IEnumerator SpawnWithDelay()
        {
            game.MergeState = MergeStateType.MergeLock;
            
            foreach (var obj in config.LevelStartMobs[save.CurrentLevel])
            {
                int totalToSpawn = obj.Value;

                int maxPerPart = 3;
                int batchSize = 0;
                if (totalToSpawn <= maxPerPart)
                {
                    batchSize = totalToSpawn;
                }
                else
                {
                    batchSize = Mathf.CeilToInt(totalToSpawn / maxPerPart); // Делаем деление на 3
                }
                
                
            
                int spawned = 0;
                while (spawned < totalToSpawn)
                {
                    int spawnThisBatch = Mathf.Min(batchSize, totalToSpawn - spawned); // Считаем, сколько осталось спавнить
                    for (int i = 0; i < spawnThisBatch; i++)
                    {
                        Vector3 rndPosition;
                        int attempts = 0;

                        do
                        {
                            rndPosition = new Vector3(
                                Random.Range(minPosition.x, maxPosition.x),
                                Random.Range(minPosition.y, maxPosition.y),
                                Random.Range(minPosition.z, maxPosition.z)
                            );
                            attempts++;
                        } while (Physics.CheckSphere(rndPosition, 0.5f) && attempts < 10);

                        var rndRotation = Quaternion.Euler(
                            Random.Range(0, 360),
                            Random.Range(0, 360),
                            Random.Range(0, 360)
                        );

                        var newObj = Instantiate(config.MergeObjects[obj.Key], rndPosition, rndRotation, container);
                        _levelObjects.Add(newObj);
                    }

                    spawned += spawnThisBatch;
                    yield return new WaitForSeconds(0.25f); // Ожидание перед следующим "пакетом"
                }
            }

            foreach (var obj in _levelObjects)
            {
                var view = obj.GetComponent<BaseView>();
                game.Fabric.InitView(view);
            }
                
            _levelObjects.Clear();
            
            game.MergeState = MergeStateType.Merge;
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