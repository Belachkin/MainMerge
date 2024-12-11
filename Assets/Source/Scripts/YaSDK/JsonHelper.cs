using UnityEngine;

namespace Source.Scripts.YaSDK
{
    public static class JsonHelper
    {
        public static string FromStringToJSON(string data)
        {
            PlayerState saveObj = new PlayerState
            {
                saveKey = data
            };
            var json = JsonUtility.ToJson(saveObj);
            return json;
        }

        public static string FromJSONToString(string json)
        {
            var playerState = JsonUtility.FromJson<PlayerState>(json);
            return playerState.saveKey;
        }
    }
    
    public class PlayerState
    {
        public string saveKey;
    }
}