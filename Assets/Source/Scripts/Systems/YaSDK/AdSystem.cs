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
    public class AdSystem : GameSystem/*WithScreen<RewardedADUIScreen>*/
    {
        [SerializeField] private float rewardCd=30;
        [SerializeField] private float interCd=60;
        
        private EcsFilter filter;
        private InterAdUIScreen interAdUIScreen;
        private Coroutine coroutineReward;
        private Coroutine coroutineInter;
        private bool interTimerReady;
        
        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<SDKEvent>().End();
            interAdUIScreen = FindObjectOfType<InterAdUIScreen>(true);
            //screen.AdButton.onClick.AddListener(OnShowReward);
            
            YandexManager.Instance.InterClosedEvent += OnInterClosed;
            
            //screen.Toggle(false);
            coroutineReward = StartCoroutine(RewardCd());
            
            coroutineInter = StartCoroutine(InterCd());
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                if (pool.SDKEvent.Get(e).SdkEventType==SdkEventType.INTER  //is inter event
                    && interTimerReady                                     //timer ready
                    && (!game.WantToAskReviewNow))  //there will not be rate us screen
                {
                    //StartCoroutine(StartPreInterPause(2f)); //2f is required by sdk!
                    StartCoroutine(CoroutineManager.WaitThenPerform(0.1f, YandexManager.Instance.ShowInter));
                }
            }
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
            //interAdUIScreen.Close();
        }

        private IEnumerator InterCd()
        {
            interTimerReady = false;
            yield return new WaitForSeconds(interCd);
            interTimerReady = true;
        }

        private void OnShowReward()
        {
            //screen.Toggle(false);
            YandexManager.Instance.ShowRewardedAd();
            coroutineReward = StartCoroutine(RewardCd());
        }

        private IEnumerator RewardCd()
        {
            yield return new WaitForSeconds(rewardCd);
            //screen.Toggle(true);
        }
    }
}