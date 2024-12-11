using UnityEngine;

namespace Snippets.Settings
{
    public class SettingsData
    {
        const string _soundKey = "game.sound.state";
        const string _hapticKey = "game.haptic.state";

        public bool SoundState { get; private set; }
        public bool HapticState { get; private set; }

        public SettingsData()
        {
            SoundState = PlayerPrefs.GetInt(_soundKey, 1) == 1;
            HapticState = PlayerPrefs.GetInt(_hapticKey, 1) == 1;
        }

        public void SwitchSound()
        {
            SoundState = !SoundState;
            SaveSound();
        }

        public void SwitchHaptic()
        {
            HapticState = !HapticState;
            SaveHaptic();
        }

        void SaveSound()
        {
            PlayerPrefs.SetInt(_soundKey, SoundState ? 1 : 0);
        }

        void SaveHaptic()
        {
            PlayerPrefs.SetInt(_hapticKey, HapticState ? 1 : 0);
        }
    }
}