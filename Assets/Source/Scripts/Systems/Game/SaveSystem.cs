using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;

namespace Source.Scripts.Systems.Game
{
    public class SaveSystem : GameSystem
    {
        private EcsFilter filter;
        
        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<SaveEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                Bootstrap.Instance.SaveGame();
                return;
            }
        }
    }
}