using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.Data;
using Source.Scripts.Helpers;
using Source.Scripts.SDK;
using Source.Scripts.UI;
using Source.Scripts.YaSDK;
using UnityEngine;

namespace Source.Scripts.Systems.YaSDK
{
     public class RateUsSystem : GameSystemWithScreen<RateUsUIScreen>
    {
        [SerializeField] private float delay=180;

        private AuthUIScreen authUIScreen;
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<SDKEvent>().End();
            authUIScreen = FindObjectOfType<AuthUIScreen>(true);
            authUIScreen.AuthButton.onClick.AddListener(OnAuthBtn);
            authUIScreen.BackButton.onClick.AddListener(OnCloseAuth);
            
            YandexManager.Instance.CanReviewCallbackEvent += OnCanReviewCallback;
            YandexManager.Instance.ReviewSuccessEvent += OnReviewSuccess;
            YandexManager.Instance.ReviewFailEvent += OnReviewFail;
            
            screen.CloseButton.onClick.AddListener(OnNoThanks);

            if (!save.PlayerSDKData.HasRated)
                StartCoroutine(CoroutineManager.WaitThenPerform(delay, EnableReviewAsk));
        }

        //used once
        private void EnableReviewAsk()
        {
            game.WantToAskReviewNow = true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //asking review
            foreach (var e in filter)
            {
                if (!save.PlayerSDKData.HasRated
                    && !game.HasAskedReviewInSession
                    && game.WantToAskReviewNow
                    && pool.SDKEvent.Get(e).SdkEventType==SdkEventType.RATE_US
                    && save.CurrentTutorStepType==TutorStepType.DONE) 
                    YandexManager.Instance.CanReviewSDK();
            }
        }
        

        private void OnCanReviewCallback(bool can, string reason)
        {
            game.HasAskedReviewInSession = true;
            game.WantToAskReviewNow = false;
            if (can || reason=="NO_AUTH")
            {
                screen.Open();
                screen.CloseButton.transform.localScale=Vector3.zero;
                StartCoroutine(CoroutineManager.WaitThenPerform(2f, ScaleCloseBtn));
                if (reason == "NO_AUTH")
                {
                    YandexManager.Instance.AuthSuccessEvent -= OnAuthSuccess;
                    YandexManager.Instance.AuthFailEvent -= OnAuthFail;
                    YandexManager.Instance.AuthSuccessEvent += OnAuthSuccess;
                    YandexManager.Instance.AuthFailEvent += OnAuthFail;
                    screen.ReviewButton.onClick.AddListener(()=>{authUIScreen.Open();});
                }
                else
                {
                    screen.ReviewButton.onClick.AddListener(()=>YandexManager.Instance.TryAskReviewSDK());
                }
            }

        }

        private void ScaleCloseBtn()
        {
            screen.CloseButton.transform.DOScale(Vector3.one, 0.5f);
        }
        
        private void OnAuthSuccess()
        {
            YandexManager.Instance.AuthSuccessEvent -= OnAuthSuccess;
            YandexManager.Instance.AuthFailEvent -= OnAuthFail;
            screen.ReviewButton.onClick.RemoveAllListeners();
            screen.ReviewButton.onClick.AddListener(()=>{YandexManager.Instance.TryAskReviewSDK();});
            authUIScreen.Close();
            pool.Save();
            Debug.Log("Auth Success");
        }
        
        private void OnAuthFail()
        {
            Debug.Log("Auth Fail");
        }

        private void OnNoThanks()
        {
            UnsubscribeAll();
            screen.Close();
        }

        private void OnReviewSuccess()
        {
            //mb add thank u message or action
            save.PlayerSDKData.HasRated = true;
            UnsubscribeAll();
            screen.Close();
            pool.Save();
            Debug.Log("Review Success");
        }
        
        private void OnReviewFail()
        {
            UnsubscribeAll();
            screen.Close();
            Debug.Log("Review Fail");
        }

        private void UnsubscribeAll()
        {
            YandexManager.Instance.AuthSuccessEvent -= OnAuthSuccess;
            YandexManager.Instance.AuthFailEvent -= OnAuthFail;
            screen.ReviewButton.onClick.RemoveAllListeners();
        }

        private void OnAuthBtn()
        {
            YandexManager.Instance.AskAuthCS();
        }

        private void OnCloseAuth()
        {
            authUIScreen.Close();
        }
    }
}