using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.Components.Events;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class MouseHoverSystem : GameSystem
    {
        private const int MaxHoverIndex = 2;
        
        private EcsFilter filter;
        private int HoverIndex = 0;
        private Camera camera;
        public override void OnInit()
        {
            base.OnInit();
            
            camera = Camera.main;
            
            filter = eventWorld.Filter<MergeEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hit))
                {
                    var baseView = hit.collider.GetComponent<BaseView>();
                
                    if (baseView != null && pool.Hoverable.Has(baseView.Entity))
                    {
                        ref var hoverable = ref pool.Hoverable.Get(baseView.Entity);

                        if (hoverable.IsHovered == false && HoverIndex < MaxHoverIndex)
                        {
                            hoverable.IsHovered = true;
                            HoverIndex++;
                        }
                        else if (hoverable.IsHovered == true)
                        {
                            hoverable.IsHovered = false;
                            HoverIndex--;
                        }
                    }
                }
            }

            foreach (var e in filter)
            {
                HoverIndex = 0;
            }
        }
    }
}