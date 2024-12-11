using System;
using Kuhpik;
using Source.Scripts.YaSDK;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class SettingsUIScreen : UIScreen
    {
        [SerializeField] private GameObject settingsBG;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button soundButton;
        [SerializeField] private Button langButton;
        

        public void Init(Action settingsToggle, Action soundToggle,Action musicToggle,Action langToggle)
        {
            settingsButton.onClick.AddListener(settingsToggle.Invoke);
            musicButton.onClick.AddListener(musicToggle.Invoke);
            soundButton.onClick.AddListener(soundToggle.Invoke);
            langButton.onClick.AddListener(langToggle.Invoke);
        }

        public void ToggleSettings()
        {
            settingsBG.SetActive(!settingsBG.activeSelf);
        }

        public void ToggleMusic(bool isEnabled)
        {
            var sprite = isEnabled ? config.MusicOn : config.MusicOff;
            musicButton.image.sprite = sprite;
        }

        public void ToggleSound(bool isEnabled)
        {
            var sprite = isEnabled ? config.SoundOn : config.SoundOff;
            soundButton.image.sprite = sprite;
        }
        
        public void ToggleLang(LangType langType)
        {
            var sprite = langType==LangType.EN ? config.EngFlag : config.RuFlag;
            langButton.image.sprite = sprite;
        }
    }
}