using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;
using Source.Scripts.Data;

namespace Kuhpik
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        // Example
        // [SerializeField] [BoxGroup("Moving")] private float moveSpeed;
        // public float MoveSpeed => moveSpeed;
        
        public List<MergeObjectType> MergeLevels = new List<MergeObjectType>();
        public SerializedDictionary<MergeObjectType, GameObject> MergeObjects;
        
        [Header("Levels")]
        public SerializedDictionary<MergeObjectType, Sprite> TaskIcons;
        public SerializedDictionary<int, SerializedDictionary<MergeObjectType, int>> LevelTasks;
        public SerializedDictionary<int, SerializedDictionary<MergeObjectType, int>> LevelStartMobs;

    }
}