using Source.Scripts.Data;
using UnityEngine;

namespace Source.Scripts.Components.Events
{
    public struct NewObjectEvent
    {
        public MergeObjectType MergeObjectType;
        public Vector3 Position;
    }
}