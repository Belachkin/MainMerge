using System.Linq;
using Leopotam.EcsLite;
using Source.Scripts.EcsUtil;
using UnityEngine;

namespace Kuhpik
{
    public abstract class GameSystem : MonoBehaviour, IGameSystem
    {
        protected SaveData save;
        protected GameData game;
        protected GameConfig config;
        protected AudioConfig audioConfig;

        protected EcsWorld world => game.World;
        protected EcsWorld eventWorld => game.EventWorld;
        protected Pools pool => game.Pools;

        public virtual void OnCustomTick() { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnGameEnd() { }

        public virtual void OnGameStart() { }

        public virtual void OnInit() { }

        public virtual void OnLateUpdate() { }

        public virtual void OnStateEnter() { }

        public virtual void OnStateExit() { }

        public virtual void OnUpdate() { }
        
                
#if UNITY_EDITOR
       
        private void OnValidate()
        {
            var n = (GetType().ToString());
            var split = n.Split('.').ToList();
            n = split[split.Count-1];
            n = System.Text.RegularExpressions.Regex.Replace(n, "[A-Z]", " $0");
            gameObject.name = n;
            transform.position = Vector3.zero;
        }
#endif
    }
}