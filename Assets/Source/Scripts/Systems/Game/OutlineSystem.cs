using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.Components.View;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class OutlineSystem : GameSystem
    {
        EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = world.Filter<OutlineComponent>().Inc<HoverableComponent>().Inc<RigidbodyComponent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var e in filter)
            {
                ref var outlineEntity = ref pool.Outline.Get(e);
                ref var hoverableEntity = ref pool.Hoverable.Get(e);
                ref var RB = ref pool.Rb.Get(e);
                
                if (hoverableEntity.IsHovered && outlineEntity.Outline.enabled == false)
                {
                    outlineEntity.Outline.enabled = true;

                    var transform = RB.Value.transform;
                    
                    RB.Value.useGravity = false;
                    RB.Value.isKinematic = true;
                    
                    Sequence sequence = DOTween.Sequence();
                    
                    sequence.Append(transform.DOMove(new Vector3(transform.position.x, 1.5f, transform.position.z), 0.1f));
                    sequence.Append(transform.DORotate(new Vector3(0, transform.eulerAngles.y, 0), 0.1f));
                    
                    sequence.AppendCallback(() =>
                         {
                             transform
                                 .DORotate(new Vector3(0, 360, 0), 2f, RotateMode.WorldAxisAdd) 
                                 .SetEase(Ease.Linear)
                                 .SetLoops(-1)
                                 .Play();
                         });
                    
                    sequence.Play();
                    
                }
                else if(hoverableEntity.IsHovered == false && outlineEntity.Outline.enabled == true)
                {
                    outlineEntity.Outline.enabled = false;
                    
                    DOTween.Kill(RB.Value.transform);
                    
                    
                    RB.Value.isKinematic = false; 
                    RB.Value.useGravity = true;
                }
                else if(hoverableEntity.IsHovered == false)
                {
                    DOTween.Kill(RB.Value.transform);
                }
            }
        }
    }
}