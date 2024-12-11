using System;
using UnityEngine;

namespace Snippets.Tutorial
{
    [Serializable]
    public abstract class TutorialStep
    {
        public event Action CompleteEvent;
        protected TutorialSystem system;

        public abstract void OnUpdate();        
        protected abstract void OnBegin();
        protected abstract void OnComplete();

        public virtual void Begin(TutorialSystem system)
        {
            this.system = system;
            Log(true);
            OnBegin();
        }

        protected virtual void Complete()
        {
            Log(false);
            OnComplete();
            CompleteEvent?.Invoke();
        }

        void Log(bool begins)
        {
            Debug.Log($"Tutorial step {this.GetType().Name} {(begins ? "begins" : "completed")}");
        }
    }
}
