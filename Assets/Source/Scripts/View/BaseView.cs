using UnityEngine;

namespace Source.Scripts.View
{
    public class BaseView : MonoBehaviour
    {
        [NaughtyAttributes.ReadOnly]
        public int Entity;

        public virtual void Die()
        {
           /*var hpBarView = GetComponentInChildren<HpBarView>();
            if (hpBarView!=null)
                Destroy(hpBarView.HpBarUIView.gameObject);
            */
            Destroy(gameObject);
        }
        
        public void ToggleActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}