using System.Collections;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.Helpers;
using Source.Scripts.SDK;
using Source.Scripts.UI;
using Source.Scripts.YaSDK;
using UnityEngine;

namespace Source.Scripts.Systems.Game
{
    public class AdSystem : GameSystem
    {
        [SerializeField] private float rewardCd=60;
        [SerializeField] private float interCd=180;
        
        private EcsFilter filter;
        private InterAdUIScreen interAdUIScreen;
        private RateUsUIScreen rateScreen;
        private AuthUIScreen authScreen;
        
        
        private Coroutine coroutineReward;
        private Coroutine coroutineInter;
        private bool interTimerReady;
        
        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<SDKEvent>().End();
            interAdUIScreen = FindObjectOfType<InterAdUIScreen>(true);
            rateScreen = FindObjectOfType<RateUsUIScreen>(true);
            authScreen = FindObjectOfType<AuthUIScreen>(true);
            
            YandexManager.Instance.InterClosedEvent += OnInterClosed;
            
            coroutineInter = StartCoroutine(InterCd());
        }

        
        private IEnumerator StartPreInterPause(float delay)
        {
            interAdUIScreen.Open();
            interAdUIScreen.AnimateCountDots(delay);
            yield return new WaitForSeconds(delay);
            YandexManager.Instance.ShowInter();
        }

        private void OnInterClosed()
        {
            coroutineInter = StartCoroutine(InterCd());
            interAdUIScreen.Close();
        }

        private IEnumerator InterCd()
        {
            interTimerReady = false;
            yield return new WaitForSeconds(interCd);
            interTimerReady = true;
            if (interTimerReady && (!game.WantToAskReviewNow) && !rateScreen.gameObject.activeSelf && !authScreen.gameObject.activeSelf)
            {
                StartCoroutine(StartPreInterPause(2f));
            }
            else
            {
                coroutineInter= StartCoroutine(InterCd());
            }
        }
    }
}