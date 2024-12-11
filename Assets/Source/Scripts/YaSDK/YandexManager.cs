using System;
using System.Runtime.InteropServices;
using Kuhpik;
using Sirenix.OdinSerializer;
using Sirenix.OdinSerializer.Utilities;
using Source.Scripts.YaSDK;
using UnityEngine;

namespace Source.Scripts.SDK
{
    public class YandexManager : Singleton<YandexManager>
    {
        [DllImport("__Internal")]
        private static extern bool IsMobile();

        [DllImport("__Internal")]
        private static extern bool AskAuth();

        [DllImport("__Internal")]
        private static extern void CanReview();

        [DllImport("__Internal")]
        private static extern void TryAskReview();

        [DllImport("__Internal")]
        private static extern void Focus();

        [DllImport("__Internal")]
        private static extern void UpdateLBExtern(int score);

        [DllImport("__Internal")]
        private static extern bool IsAuth();

        [DllImport("__Internal")]
        private static extern string GetLang();

        [DllImport("__Internal")]
        private static extern string StartGetOtherGamesURL();

        [DllImport("__Internal")]
        private static extern void SaveExtern(string data);

        [DllImport("__Internal")]
        private static extern void LoadExtern();

        [DllImport("__Internal")]
        private static extern void GameReadyAPIReadyExtern();

        [DllImport("__Internal")]
        private static extern void ShowRewardedAdExtern();
        
        [DllImport("__Internal")]
        private static extern void ShowInterExtern();

        public bool IsMobileCS;
        public bool IsAuthCS;
        public bool IsSDKLoaded;
        public string OtherGamesURL;

        public event Action AuthSuccessEvent;
        public event Action AuthFailEvent;

        public event Action<bool, string> CanReviewCallbackEvent;
        public event Action ReviewSuccessEvent;
        public event Action ReviewFailEvent;

        public event Action ResumeGameEvent;
        public event Action PauseGameEvent;

        public event Action RewardClaimEvent;
        
        public event Action InterOpenEvent;
        public event Action InterClosedEvent;
        
        private bool isNeedReward;

        public void Init()
        {
            DontDestroyOnLoad(this);
            try
            {
                IsMobileCS = IsMobile();
            }
            catch (EntryPointNotFoundException e)
            {
                Debug.LogWarning("IsMobile failed. Make sure you are running a WebGL build in a browser:" +
                                 e.Message);
            }

            try
            {
                IsAuthCS = IsAuth();
                Debug.Log("auth "+IsAuthCS);
            }
            catch (EntryPointNotFoundException e)
            {
                Debug.LogWarning("IsAuth failed. Make sure you are running a WebGL build in a browser:" +
                                 e.Message);
            }
            
            try
            {
                StartGetOtherGamesURL();
            }
            catch (EntryPointNotFoundException e)
            {
                Debug.LogWarning("StartGetOtherGamesURL failed. Make sure you are running a WebGL build in a browser:" +
                                 e.Message);
            }

            ResumeGameEvent += OnResumeGame;
            PauseGameEvent += OnPauseGame;
        }

        private void OnResumeGame()
        {
#if !UNITY_EDITOR
            Focus();
#endif
            Time.timeScale = 1;

            AudioListener.pause = false;
            // _isAdsPaused = false;
        }

        private void OnPauseGame()
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            //_isAdsPaused = true;
        }

        //is called from html
        public void OnVisibilityGameWindow(string visible)
        {
            if (visible == "true")
                ResumeGameEvent?.Invoke();
            else
                PauseGameEvent?.Invoke();
        }

        #region Ads
        //--------reward
        public void ShowRewardedAd()
        {
#if !UNITY_EDITOR
            ShowRewardedAdExtern();
#else
            RewardedAdShown();
            RewardedAdClosed();
#endif
        }

        //is called from html
        public void RewardedAdOpen()
        {
            OnPauseGame();
        }

        //is called from html
        public void RewardedAdClosed()
        {
            OnResumeGame();
            if (isNeedReward) 
                RewardClaimEvent?.Invoke();
            isNeedReward = false;
        }

        //is called from html
        public void RewardedAdShown()
        {
            isNeedReward = true;
        }
        
        //------------inter
        public void ShowInter()
        {
#if !UNITY_EDITOR
            ShowInterExtern();
#else
            InterOpen();
            InterClosed("true");
#endif
        }

        public void InterOpen()
        {
            InterOpenEvent?.Invoke();
            OnPauseGame();
        }
        
        public void InterClosed(string wasShown)
        {
            InterClosedEvent?.Invoke();
            if (wasShown == "true"){}
            OnResumeGame();
        }

        #endregion

        public void UpdateLB(int score)
        {
#if !UNITY_EDITOR
            UpdateLBExtern(score);
#endif
        }


        public void GameReadyAPIReady()
        {
#if !UNITY_EDITOR
            GameReadyAPIReadyExtern();
#endif
        }

        #region Review_methods

        public void CanReviewSDK()
        {
#if !UNITY_EDITOR
            CanReview();
#endif
        }

        public void TryAskReviewSDK()
        {
            TryAskReview();
        }

        //called from sdk
        public void FinishAskReview(string feedback)
        {
            if (feedback == "true")
                ReviewSuccessEvent?.Invoke();
            else
                ReviewFailEvent?.Invoke();
        }

        //called from sdk
        public void FinishCanReview(string reason)
        {
            CanReviewCallbackEvent?.Invoke(reason == "true", reason);
        }

        #endregion

        #region Auth_methods

        public void AuthSuccess()
        {
            IsAuthCS = true;
            AuthSuccessEvent?.Invoke();
        }

        public void AuthFail()
        {
            AuthFailEvent?.Invoke();
        }

        public void AskAuthCS()
        {
            AskAuth();
        }

        #endregion

        //called from html
        public void SetOtherGamesUrl(string url)
        {
            OtherGamesURL = url;
        }

        public LangType GetLangSDK()
        {
            try
            {
                var s = GetLang();
                return s == "en" ? LangType.EN : LangType.RU;
            }
            catch (EntryPointNotFoundException e)
            {
                Debug.LogWarning("GetLangSDK failed. Make sure you are running a WebGL build in a browser:" +
                                 e.Message);
            }

            return LangType.EN;
        }
        
        //called from html
        public void SDKLoaded()
        {
            IsSDKLoaded = true;
        }

        public static void Save<T>(T value, string key)
        {
            byte[] bytes = SerializationUtility.SerializeValue(value, DataFormat.Binary);
            var base64String = Convert.ToBase64String(bytes);
            var json = JsonHelper.FromStringToJSON(base64String);
            SaveExtern(json);
        }

        public static void StartLoadFromSDK()
        {
            LoadExtern();
        }
        
        //called from html
        public void FinishLoadFromSDK(string data)
        {
            var playerDataInstaller = FindObjectOfType<PlayerDataInstaller>();
            data = JsonHelper.FromJSONToString(data);
            if (!data.IsNullOrWhitespace())
            {
                byte[] bytes = Convert.FromBase64String(data);
                var saveData = SerializationUtility.DeserializeValue<SaveData>(bytes, DataFormat.Binary);
                playerDataInstaller.SetDataFromSDK(saveData);
            }
            else
            {
                playerDataInstaller.SetDataFromSDK(new SaveData());
            }
        }
    }
}