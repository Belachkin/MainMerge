using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.Components.Events;
using Source.Scripts.Components.Life;
using Source.Scripts.Components.Move;
using Source.Scripts.Components.View;

namespace Source.Scripts.EcsUtil
{
    public class Pools
    {
        private EcsWorld eventWorld;
        //world components
        public readonly EcsPool<BaseViewComponent> View;
        public readonly EcsPool<Direction> Dir;
        public readonly EcsPool<Speed> Speed;
        public readonly EcsPool<MaxSpeed> MaxSpeed;
        public readonly EcsPool<RigidbodyComponent> Rb;
        //public readonly EcsPool<Hp> Hp;
        public readonly EcsPool<AnimComponent> Anim;
        public readonly EcsPool<DeadTag> DeadTag;
        
        public readonly EcsPool<HoverableComponent> Hoverable;
        public readonly EcsPool<OutlineComponent> Outline;
        
        public readonly EcsPool<MergeTypeComponent> MergeType;
        
        //event world components
        public readonly EcsPool<SoundEvent> SoundEvent;
        public readonly EcsPool<SaveEvent> SaveEvent;
        public readonly EcsPool<SDKEvent> SDKEvent;
        
        public readonly EcsPool<MergeEvent> MergeEvent;
        public readonly EcsPool<NewObjectEvent> NewObjectEvent;
        public readonly EcsPool<LevelCompleteEvent> LevelCompleteEvent;
        public readonly EcsPool<StartLevelEvent> StartLevelEvent;
        
        public readonly EcsPool<AddMoneyEvent> AddMoneyEvent;
        public readonly EcsPool<UpdateMoneyEvent> UpdateMoneyEvent;
        public Pools(EcsWorld world, EcsWorld eventWorld)
        {
            this.eventWorld = eventWorld;
            //world components
            View = world.GetPool<BaseViewComponent>();
            Dir = world.GetPool<Direction>();
            Speed = world.GetPool<Speed>();
            MaxSpeed = world.GetPool<MaxSpeed>();
            Rb = world.GetPool<RigidbodyComponent>();
            //Hp = world.GetPool<Hp>();
            Anim = world.GetPool<AnimComponent>();
            DeadTag = world.GetPool<DeadTag>();
            
            Hoverable = world.GetPool<HoverableComponent>();
            Outline = world.GetPool<OutlineComponent>();
            MergeType = world.GetPool<MergeTypeComponent>();
            
            //event world components
            SoundEvent = eventWorld.GetPool<SoundEvent>();
            SaveEvent = eventWorld.GetPool<SaveEvent>();
            SDKEvent = eventWorld.GetPool<SDKEvent>();
            
            MergeEvent = eventWorld.GetPool<MergeEvent>();
            NewObjectEvent = eventWorld.GetPool<NewObjectEvent>();
            LevelCompleteEvent = eventWorld.GetPool<LevelCompleteEvent>();
            StartLevelEvent = eventWorld.GetPool<StartLevelEvent>();
            AddMoneyEvent = eventWorld.GetPool<AddMoneyEvent>();
            UpdateMoneyEvent = eventWorld.GetPool<UpdateMoneyEvent>();
        }
        
        public void Save()
        {
            SaveEvent.Add(eventWorld.NewEntity());
        }
    }
}