using Nrjwolf.Tools.AttachAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class CounterUIView : MonoBehaviour
    {
        [GetComponentInChildren()] [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField] private Image icon;

        public void SetText(int value)
        {
            text.text = $"{value}";
        }
        public void SetText(string s)
        {
            text.text = s;
        }
        
        public void SetText(int value,int maxValue)
        {
            text.text = $"{value}/{maxValue}";
        }

        public void SetResImage(Sprite sprite)
        {
            icon.sprite = sprite;
        }
    }
}