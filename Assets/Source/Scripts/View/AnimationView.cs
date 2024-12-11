using UnityEngine;

namespace Source.Scripts.View
{
    public class AnimationView : MonoBehaviour
    {
        private Animator visualAnimator;
        [SerializeField] private GameObject visual;
      
        public bool BlockAnim;

        private void Awake()
        {
            visualAnimator = visual.GetComponentInChildren<Animator>();
        }

        public void Play(string name)
        {
            if (!gameObject.activeSelf || BlockAnim)
                return;

            visualAnimator.Play(name);
        }
    }
}