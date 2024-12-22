using Kuhpik;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Screens
{
    public class FloatingTextUIScreen : UIScreen
    {
        public Canvas canvas;
        public float fadeDuration = 0.5f; 
        public float moveDistance = 50f; 
        public float lifetime = 1f;
        
        public GameObject FloatingTextObject;
        public TextMeshProUGUI FloatingText;
    }
}