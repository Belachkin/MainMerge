using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Source.Scripts.Data;
using Source.Scripts.YaSDK;

namespace Kuhpik
{
    /// <summary>
    /// Used to store player's data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        // Example (I use public fields for data, but u free to use properties\methods etc)
        // [BoxGroup("level")] public int level;
        // [BoxGroup("currency")] public int money;
        public PlayerSDKData PlayerSDKData=new PlayerSDKData();
        
        public bool MusicOn = true;
        public bool SoundOn = true;
        public LangType LangType;

        public TutorStepType CurrentTutorStepType = TutorStepType.MERGE_1;

        public int CurrentLevel = 0;
        public int MaxLevel = 10;
        //public bool VibroOn = true;

        public int Money = 150;
    }
}