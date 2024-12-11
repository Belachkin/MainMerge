using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.Data;
using Source.Scripts.SDK;
using UnityEngine;


namespace Source.Scripts.Systems.Game
{
    public class AudioSystem : GameSystem
    {
        [SerializeField] private AudioSource music;
        //[SerializeField] private AudioSource waterfall;
        [SerializeField] private AudioSource money;
        [SerializeField] private AudioSource[] sounds;
        [SerializeField] private float maxPitch;
        [SerializeField] private float delayBeforePitchReset;
        
        private EcsFilter filter;
        private Coroutine moneyPitchCor;
        private float delayInc;
        private float musicTime;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<SoundEvent>().End();
            //music.clip = bg music
            //music.Play();
            YandexManager.Instance.PauseGameEvent += OnGamePause;
            YandexManager.Instance.ResumeGameEvent += OnResumeGame;
        }

        private void OnGamePause()
        {
            musicTime=music.time;
            music.Pause();
        }
        private void OnResumeGame()
        {
            music.time = musicTime;
            music.Play();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                var soundEvent = pool.SoundEvent.Get(e);
                
                //is sound muted
                if (!soundEvent.IsMusic && !save.SoundOn)
                    continue;
                //is music muted
                if (soundEvent.IsMusic && !save.MusicOn)
                    continue;

                AudioSource source;
                if (soundEvent.IsMusic)
                {
                    source = music;
                }else if (soundEvent.AudioClip==audioConfig.Money)
                {
                    source = money;
                    if (money.pitch<maxPitch) 
                        money.pitch += 0.0005f;

                    delayInc = 0;
                    if (moneyPitchCor==null) 
                        moneyPitchCor = StartCoroutine(MoneyPitchCor());
                }
                else
                {
                    source= GetFreeSoundSource();
                }

                source.clip = soundEvent.AudioClip;
                source.Play();
            }
        }

        private IEnumerator MoneyPitchCor()
        {
            while (delayInc < delayBeforePitchReset)
            {
                delayInc += Time.deltaTime;
                yield return null;
            }
            
            money.pitch = 1f;
            moneyPitchCor = null;
        }

        private AudioSource GetFreeSoundSource()
        {
            foreach (var sound in sounds)
            {
                if (!sound.isPlaying)
                    return sound;
            }

            //return 1st if all are busy
            return sounds[0];
        }
    }
}