using System.Collections;
using NaughtyAttributes;
using Source.Scripts.SDK;
using UnityEngine;

namespace Kuhpik
{
    public class PlayerDataInstaller : Installer
    {
        [SerializeField] bool isTesting;
        [SerializeField] [ShowIf("isTesting")] SaveData testData;

        public override int Order => 2;

        const string saveKey = "saveKey";
        SaveData data;

        public override void Process()
        {
            data = HandlePlayerData();

#if UNITY_EDITOR
            Bootstrap.Instance.itemsToInject.Add(data);
            Bootstrap.Instance.IsPlayerDataLoaded = true;
#else
            if (!YandexManager.Instance.IsAuthCS)
            {
                Bootstrap.Instance.itemsToInject.Add(data);
                Bootstrap.Instance.IsPlayerDataLoaded = true;
            }
#endif
            Bootstrap.Instance.EventSave += Save;
        }

        public void SetDataFromSDK(SaveData saveData)
        {
            data = saveData;
            Bootstrap.Instance.itemsToInject.Add(data);
            Bootstrap.Instance.IsPlayerDataLoaded = true;
        }


        SaveData HandlePlayerData()
        {
#if UNITY_EDITOR
            return isTesting ? testData : Load();
#else
            return Load();
#endif
        }

        void Save()
        {
#if UNITY_EDITOR
            SaveExtension.Save(data, saveKey);
#else     
            if (YandexManager.Instance.IsAuthCS)
            {
                YandexManager.Save(data, saveKey);
                Debug.Log("save to cloud");
            }
            else
            {
                SaveExtension.Save(data, saveKey);
                Debug.Log("save to client");
            }
                
#endif
        }

        SaveData Load()
        {
#if UNITY_EDITOR
            return SaveExtension.Load(saveKey, new SaveData());
#else
            if (YandexManager.Instance.IsAuthCS)
            {
                Debug.Log("load from cloud");
                YandexManager.StartLoadFromSDK();
                return new SaveData();
            }
            else
            {
                Debug.Log("load from client");
                return SaveExtension.Load(saveKey, new SaveData());
            }

#endif
        }
    }
}