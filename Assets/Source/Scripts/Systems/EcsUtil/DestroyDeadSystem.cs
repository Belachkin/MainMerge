using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Life;

namespace Source.Scripts.Systems.EcsUtil
{
    public class DestroyDeadSystem : GameSystem
    {
        private EcsFilter filter;
        
        public override void OnInit()
        {
            base.OnInit();
            filter = world.Filter<DeadTag>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filter)
            {
                if (pool.View.Has(ent))
                {
                    pool.View.Get(ent).Value.Die();
                    world.DelEntity(ent);
                }
            }
        }

    }
}