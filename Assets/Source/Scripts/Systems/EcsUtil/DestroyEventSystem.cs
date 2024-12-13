using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;

namespace Source.Scripts.Systems.EcsUtil
{
    public class DestroyEventSystem : GameSystem
    {
        private Dictionary<EcsFilter,IEcsPool> filtersPools;

        public override void OnInit()
        {
            base.OnInit(); 
            
            filtersPools = new ();
            //AddFilter<DamageEvent>();
            AddFilter<SoundEvent>();
            AddFilter<SDKEvent>();
            AddFilter<SaveEvent>();
            AddFilter<MergeEvent>();
            AddFilter<NewObjectEvent>();
            AddFilter<LevelCompleteEvent>();
            AddFilter<StartLevelEvent>();
            AddFilter<AddMoneyEvent>();
        }

        private void AddFilter<T>() where T: struct
        {
            filtersPools.Add(eventWorld.Filter<T>().End(),eventWorld.GetPool<T>());
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var filter in filtersPools.Keys)
            {
                foreach (var ent in filter)
                {
                    filtersPools[filter].Del(ent);
                }
            }
           

        }
    }
}