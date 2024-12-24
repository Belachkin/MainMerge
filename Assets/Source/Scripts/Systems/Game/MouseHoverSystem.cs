using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.Components.Events;
using Source.Scripts.Data;
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
        
        [SerializeField] private Transform mergeOnjectsContainer;
        
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
            Vector3 fingerPosition = Vector3.zero;

            // Устанавливаем позицию пальца в зависимости от текущего шага
            if (save.CurrentLevel == 0 && save.CurrentTutorStepType == TutorStepType.MERGE_1)
            {
                fingerPosition = mergeOnjectsContainer.GetChild(0).position;
            }
            else if (save.CurrentLevel == 0 && save.CurrentTutorStepType == TutorStepType.MERGE_2)
            {
                fingerPosition = mergeOnjectsContainer.GetChild(1).position;
            }
            
            // Проверяем нажатие мыши
            if (Input.GetMouseButtonDown(0) && 
                save.CurrentTutorStepType != TutorStepType.BONUS_AUTOMERGE && 
                save.CurrentTutorStepType != TutorStepType.BONUS_TNT&&
                game.MergeState == MergeStateType.Merge)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (var hit in hits)
                {
                    var baseView = hit.collider.GetComponent<BaseView>();

                    if (baseView != null && pool.Hoverable.Has(baseView.Entity))
                    {
                        
                        pool.SoundEvent.Add(eventWorld.NewEntity()).AudioClip = audioConfig.SelectSound;
                        
                        ref var hoverable = ref pool.Hoverable.Get(baseView.Entity);
                        
                        // Проверяем, совпадает ли объект с ожидаемым
                        bool isCorrectObject = false;

                        if (save.CurrentTutorStepType == TutorStepType.MERGE_1)
                        {
                            isCorrectObject = baseView.transform == mergeOnjectsContainer.GetChild(0);
                        }
                        else if (save.CurrentTutorStepType == TutorStepType.MERGE_2)
                        {
                            isCorrectObject = baseView.transform == mergeOnjectsContainer.GetChild(1);
                        }

                        if (!hoverable.IsHovered && HoverIndex < MaxHoverIndex)
                        {
                            // Выделение корректного объекта
                            hoverable.IsHovered = true;
                            HoverIndex++;

                            if (isCorrectObject)
                            {
                                if (save.CurrentTutorStepType == TutorStepType.MERGE_1)
                                {
                                    save.CurrentTutorStepType = TutorStepType.MERGE_2;
                                }
                                else if (save.CurrentTutorStepType == TutorStepType.MERGE_2)
                                {
                                    save.CurrentTutorStepType = TutorStepType.WAIT_BONUS_AUTOMERGE;
                                }
                            }
                        }
                        else if (hoverable.IsHovered)
                        {
                            // Снимаем выделение, если объект уже был выделен
                            hoverable.IsHovered = false;
                            HoverIndex--;

                            fingerPosition = baseView.transform.position;

                            if (save.CurrentTutorStepType == TutorStepType.MERGE_2 &&
                                fingerPosition == mergeOnjectsContainer.GetChild(0).position)
                            {
                                save.CurrentTutorStepType = TutorStepType.MERGE_1;
                            }
                            else if(save.CurrentTutorStepType == TutorStepType.MERGE_1 && 
                                    fingerPosition == mergeOnjectsContainer.GetChild(1).position)
                            {
                                save.CurrentTutorStepType = TutorStepType.MERGE_1;
                            }
                        }

                        break;
                    }
                }
            }

            // Обновляем позицию пальца только при необходимости
            if (save.CurrentTutorStepType == TutorStepType.MERGE_1 || save.CurrentTutorStepType == TutorStepType.MERGE_2)
            {
                pool.FingerSetPositionEvent.Add(eventWorld.NewEntity()).Position = fingerPosition;
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