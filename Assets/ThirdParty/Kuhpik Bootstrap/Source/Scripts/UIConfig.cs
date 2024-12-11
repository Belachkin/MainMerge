using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;
using Source.Scripts.YaSDK;

namespace Kuhpik
{
    [CreateAssetMenu(menuName = "Config/UIConfig")]
    public sealed class UIConfig : ScriptableObject
    {
        // Example
        // [SerializeField] [BoxGroup("Moving")] private float moveSpeed;
        // public float MoveSpeed => moveSpeed;
        [Header("Settings")]
        public Sprite EngFlag;
        public Sprite RuFlag;
        public Sprite SoundOn;
        public Sprite SoundOff;
        public Sprite MusicOn;
        public Sprite MusicOff;
        [Header("Money")]
        public Sprite MoneyActive;
        public Sprite MoneyNotActive;
        public Sprite ButtonBuyActive;
        public Sprite ButtonBuyNotActive;
        
        public SerializedDictionary<LangType,SerializedDictionary<UITextType, string>> UiNames;
    }
}