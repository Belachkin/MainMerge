using System;
using UnityEngine;
using UnityEngine.UI;

namespace Snippets.Settings
{
    // You can inherit this script from UIScreen if you want to.
    public class SettingsUI : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] Sprite _soundEnabledSprite;
        [SerializeField] Sprite _soundDisabledSprite;
        [SerializeField] Sprite _hapticEnabledSprite;
        [SerializeField] Sprite _hapticDisabledSprite;

        [Header("UI")]
        [SerializeField] Button _soundButton;
        [SerializeField] Button _hapticButton;
        [SerializeField] Button _closeButton;

        public event Action<bool> OnSoundStateChanged;
        public event Action<bool> OnHapticStateChanged;

        public bool SoundState => _data.SoundState;
        public bool HapticState => _data.HapticState;

        public Button CloseButton => _closeButton;
        
        SettingsData _data;

        void Start() 
        {
            _data = new SettingsData();

            _soundButton.onClick.AddListener(SwitchSound);
            _hapticButton.onClick.AddListener(SwitchHaptic);

            UpdateSoundVisual();
            UpdateHapticVisual();
        }

        void SwitchSound()
        {
            _data.SwitchSound();
            OnSoundStateChanged?.Invoke(SoundState);
            UpdateSoundVisual();
        }

        void SwitchHaptic()
        {
            _data.SwitchHaptic();
            OnHapticStateChanged?.Invoke(HapticState);
            UpdateHapticVisual();
        }

        void UpdateSoundVisual()
        {
            _soundButton.image.sprite = SoundState ? _soundEnabledSprite : _soundDisabledSprite;
        }

        void UpdateHapticVisual()
        {
            _hapticButton.image.sprite = HapticState ? _hapticEnabledSprite : _hapticDisabledSprite;
        }
    }
}
