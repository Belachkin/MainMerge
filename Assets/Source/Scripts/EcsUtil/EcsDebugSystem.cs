using Kuhpik;
using Leopotam.EcsLite;

namespace Source.Scripts.EcsUtil
{
    public class EcsDebugSystem : GameSystem
    {
        private EcsSystems _systems;

        public override void OnInit()
        {
            base.OnInit();
            _systems = new EcsSystems (game.World);
            _systems
#if UNITY_EDITOR
                // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
                // Регистрируем отладочные системы по контролю за текущей группой систем. 
                .Add (new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem ())
#endif
                .Init ();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems!=null)
            {
                _systems.Destroy();
                _systems = null;
            }
        }
    }
}