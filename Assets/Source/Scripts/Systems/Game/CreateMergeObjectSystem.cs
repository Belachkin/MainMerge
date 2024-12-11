using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class CreateMergeObjectSystem : GameSystem
    {
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<NewObjectEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                var newObjectType = pool.NewObjectEvent.Get(e);

                var newObject = Instantiate(config.MergeObjects[newObjectType.MergeObjectType],
                    newObjectType.Position, Quaternion.identity);
                
                var view = newObject.GetComponent<BaseView>();
                
                game.Fabric.InitView(view);
            }
        }
    }
}