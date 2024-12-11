
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Source.Scripts.Helpers
{
    public class PlayerPrefsManager : MonoBehaviour
    {
#if UNITY_EDITOR
       
        [ExecuteInEditMode]
        public void ClearPlayerPref()
        {
            PlayerPrefs.DeleteAll();
        }
        
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerPrefsManager))]
    class PlayerPrefManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var o = (PlayerPrefsManager) target;
            if (o == null) return;

            Undo.RecordObject(o, "PlayerPrefManager");

            if (GUILayout.Button("Clear Player Prefs"))
            {
                o.ClearPlayerPref();
            }
          
        }
    }
#endif
}