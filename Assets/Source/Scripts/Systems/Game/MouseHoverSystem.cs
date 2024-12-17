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
        private EcsFilter filterClearSeclectedEvent;
        private EcsFilter filterHovebles;
        private int HoverIndex = 0;
        private Camera camera;
        public override void OnInit()
        {
            base.OnInit();
            
            camera = Camera.main;
            
            filter = eventWorld.Filter<MergeEvent>().End();
            filterClearSeclectedEvent = eventWorld.Filter<ClearSelectedEvent>().End();
            filterHovebles = world.Filter<HoverableComponent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
    
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (var hit in hits)
                {
                    var baseView = hit.collider.GetComponent<BaseView>();
        
                    if (baseView != null && pool.Hoverable.Has(baseView.Entity))
                    {
                        ref var hoverable = ref pool.Hoverable.Get(baseView.Entity);

                        if (!hoverable.IsHovered && HoverIndex < MaxHoverIndex)
                        {
                            hoverable.IsHovered = true;
                            HoverIndex++;
                        }
                        else if (hoverable.IsHovered)
                        {
                            hoverable.IsHovered = false;
                            HoverIndex--;
                        }
                        
                        break;
                    }
                }
            }

            foreach (var e in filter)
            {   
                ClearAll();
            }

            foreach (var e in filterClearSeclectedEvent)
            {
                ClearAll();
            }
            
            
        }

        public void ClearAll()
        {
            foreach (var e in filterHovebles)
            {
                pool.Hoverable.Get(e).IsHovered = false;
            }
            
            HoverIndex = 0;
        }
    }
}