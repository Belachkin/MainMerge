using UnityEngine;
using NaughtyAttributes;

namespace Kuhpik
{
    [CreateAssetMenu(menuName = "Config/AudioConfig")]
    public sealed class AudioConfig : ScriptableObject
    {
        public AudioClip Money;
        public AudioClip ClickSound;
        public AudioClip BuySound;
        
        public AudioClip VictorySound;
        public AudioClip TaskCompleted;
        
        public AudioClip SelectSound;
        public AudioClip MergeSound;

        [Header("TNT")] 
        public AudioClip TNTActivation;
        public AudioClip TNTExplosion;
    }
}