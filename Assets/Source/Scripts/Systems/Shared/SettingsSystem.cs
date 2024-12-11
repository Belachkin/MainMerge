using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Kuhpik;
using Leopotam.EcsLite;
using MoreMountains.NiceVibrations;
using Source.Scripts.Components.Events;
using Source.Scripts.SDK;

using Source.Scripts.UI;
using Source.Scripts.YaSDK;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

namespace Source.Scripts.Systems.Shared
{
    public class SettingsSystem : GameSystemWithScreen<SettingsUIScreen>
    {
        [SerializeField] private AudioMixerGroup mixer;
        
        private List<TranslatableText> constUITexts;

        public override void OnInit()
        {
            base.OnInit();
            var findObjectsOfType = FindObjectsOfType<TranslatableText>(true);
            constUITexts = new List<TranslatableText>(findObjectsOfType);
            
            screen.Init( ToggleSettings, ToggleSound, ToggleMusic,ToggleLang);
            screen.ToggleMusic(save.MusicOn);
            screen.ToggleSound(save.SoundOn);
            
            foreach (var constUIText in constUITexts)
                constUIText.Text.text = uiConfig.UiNames[save.LangType][constUIText.UiTextType];
            screen.ToggleLang(save.LangType);

            mixer.audioMixer.SetFloat("MusicVolume", save.MusicOn ? 0 : -80);
            mixer.audioMixer.SetFloat("SoundVolume", save.SoundOn ? 0 : -80);
        }

        private void ToggleSettings()
        {
            screen.ToggleSettings();
            
        }

        private void ToggleLang()
        {
            if (save.LangType == LangType.RU)
                save.LangType = LangType.EN;
            else
                save.LangType = LangType.RU;

            //update texts
            foreach (var constUIText in constUITexts)
                constUIText.Text.text = uiConfig.UiNames[save.LangType][constUIText.UiTextType];
            screen.ToggleLang(save.LangType);
            
            pool.Save();
        }

       /* private void ToggleHaptic()
        {
            save.VibroOn = !save.VibroOn;
            screen.ToggleHaptic(save.VibroOn);
            pool.Save();
        }*/

        private void ToggleSound()
        {
            save.SoundOn = !save.SoundOn;
            screen.ToggleSound(save.SoundOn);
            mixer.audioMixer.SetFloat("SoundVolume", save.SoundOn ? 0 : -80);
            pool.Save();
        }
        
        private void ToggleMusic()
        {
            save.MusicOn = !save.MusicOn;
            screen.ToggleMusic(save.MusicOn);
            mixer.audioMixer.SetFloat("MusicVolume", save.MusicOn ? 0 : -80);
            pool.Save();
        }
    }
}