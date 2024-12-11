using UnityEngine;

namespace Source.Scripts.View.DataInitViewComponents
{
    public class DestroyComponentOnInitView : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(this);
        }
    }
}