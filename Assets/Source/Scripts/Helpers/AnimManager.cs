using DG.Tweening;
using UnityEngine;

namespace Source.Scripts.Helpers
{
    public class AnimManager
    {
        public void Pop(Transform tr)
        {
            float time = 0.15f;
            DOTween.Sequence()
                .Append(tr.DOScale(Vector3.one * 1.2f,time))
                .Append(tr.DOScale(Vector3.one,time));
        }
        
        public void Press(Transform tr)
        {
            float time = 0.15f;
            DOTween.Sequence()
                .Append(tr.DOScale(Vector3.one * 0.8f,time))
                .Append(tr.DOScale(Vector3.one,time));
        }
    }
}