using System.Collections.Generic;
using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.Components.View;
using Source.Scripts.Data;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class MergeSystem : GameSystem
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        EcsFilter filter;
        
        public override void OnInit()
        {
            base.OnInit();
            filter = world.Filter<HoverableComponent>().Inc<MergeTypeComponent>().Inc<RigidbodyComponent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            foreach (var e1 in filter)
            {
                ref var hover1 = ref pool.Hoverable.Get(e1);
                ref var mergeType1 = ref pool.MergeType.Get(e1);
                ref var rb1 = ref pool.Rb.Get(e1);

                if (!hover1.IsHovered)
                    continue;

                foreach (var e2 in filter)
                {
                    if (e1 == e2)
                        continue;

                    ref var hover2 = ref pool.Hoverable.Get(e2);
                    ref var mergeType2 = ref pool.MergeType.Get(e2);
                    ref var rb2 = ref pool.Rb.Get(e2);

                    if (hover2.IsHovered && mergeType1.MergeType == mergeType2.MergeType)
                    {
                        Debug.Log(hover1.IsHovered + " " + hover2.IsHovered);
                        
                        // hover1.IsHovered = false;
                        // hover2.IsHovered = false;
                        
                        
                        Vector3 pos1 = rb1.Value.transform.position;
                        Vector3 pos2 = rb2.Value.transform.position;
                        
                        Vector3 middlePoint = (pos1 + pos2) / 2;
                        
                        
                        MoveAndAnimate(rb1.Value.transform, middlePoint, e1, true, mergeType1.MergeType);
                        MoveAndAnimate(rb2.Value.transform, middlePoint, e2, false, mergeType2.MergeType);
                        
                        
                        
                        pool.Hoverable.Del(e1);
                        pool.MergeType.Del(e1);
                        pool.Rb.Del(e1);
                        
                        pool.Hoverable.Del(e2);
                        pool.MergeType.Del(e2);
                        pool.Rb.Del(e2);
                        
                        
                        
                        return;
                    }
                    else if(hover2.IsHovered && hover1.IsHovered && mergeType1.MergeType != mergeType2.MergeType)
                    {
                        hover1.IsHovered = false;
                        hover2.IsHovered = false;
                        //
                        // // rb1.Value.isKinematic = false;
                        // // rb2.Value.isKinematic = false;
                        
                        
                        pool.ClearSelectedEvent.Add(eventWorld.NewEntity());

                        // ApplyRepulsion(rb1.Value, rb2.Value);
                        
                        //TODO: Допилить
                        rb2.Value.AddForce(new Vector3(0f, 1f, 0f), ForceMode.Impulse);
                        
                        Debug.Log($"Объекты {rb1.Value.name} и {rb2.Value.name} не слились и разлетелись.");
                        return;
                    }
                }
            }

            
        }
        
        private void MoveAndAnimate(Transform target, Vector3 destination, int entity, bool isInstantiated, MergeObjectType mergeType)
        {
            target.DOMove(destination, 1.5f) 
                .SetEase(Ease.OutElastic); 

            target.DOScale(target.transform.localScale * 0.75f, 0.25f) 
                .SetEase(Ease.OutElastic)
                .OnComplete(() =>
                {
                    
                    Vector3 position = target.position;

                    _particleSystem.transform.position = position;
                    _particleSystem.Play();
                    
                    pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = audioConfig.MergeSound;
                    
                    // UnityEngine.Object.Destroy(target.gameObject); 
                    pool.DeadTag.Add(entity);
                    UnityEngine.Debug.Log($"Объект {target.name} уничтожен.");

                    if (isInstantiated)
                    {
                        
                        int index = config.MergeLevels.IndexOf(mergeType);
                        
                        if (config.MergeLevels.Count >= index + 1)
                        {
                            var newEntity =  eventWorld.NewEntity();
                            ref var eventComponent = ref pool.NewObjectEvent.Add(newEntity);

                            eventComponent.MergeObjectType = config.MergeLevels[index + 1];
                            eventComponent.Position = position;


                        }

                        // pool.MergeEvent.Add(eventWorld.NewEntity()).MergeType = mergeType;
                    }
                    
                    pool.MergeEvent.Add(eventWorld.NewEntity()).MergeType = mergeType;
                });
        }
        
        private void ApplyRepulsion(Rigidbody rb1, Rigidbody rb2)
        {
            // Направление отталкивания
            Vector3 direction1 = (rb1.position - rb2.position).normalized;
            Vector3 direction2 = -direction1;

            // Сила отталкивания
            float repulsionForce = 5f;

            // Применяем силу к объектам
            
            
            rb1.AddForce(direction1 * repulsionForce, ForceMode.Impulse);
            rb2.AddForce(direction2 * repulsionForce, ForceMode.Impulse);
            
            
        }
    }
}