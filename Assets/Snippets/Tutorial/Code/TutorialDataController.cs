using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Snippets.Tutorial
{
    public class TutorialDataController
    {
        readonly TutorialData data;
        readonly HashSet<string> nodesMap;

        const string saveKey = "TutorialDataController";

        public readonly ReadOnlyCollection<string> CompletedNodes;

        public TutorialDataController()
        {
            if (PlayerPrefs.HasKey(saveKey))
            {
                string json = PlayerPrefs.GetString(saveKey);
                data = JsonUtility.FromJson<TutorialData>(json);
            }

            else
            {
                data = new TutorialData();
            }

            CompletedNodes = new ReadOnlyCollection<string>(data.CompletedNodes);
            nodesMap = new HashSet<string>(data.CompletedNodes);
        }

        public void CompleteNode(string key)
        {
            Debug.Log($"Node with key: {key} completed");
            data.CompletedNodes.Add(key);
            nodesMap.Add(key);
            Save();
        }

        public bool HasKey(string key)
        {
            return nodesMap.Contains(key);
        }

        public void Reset()
        {
            data.CompletedNodes.Clear();
            nodesMap.Clear();
        }

        void Save()
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(saveKey, json);
        }
    }
}
