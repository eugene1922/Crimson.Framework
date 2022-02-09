using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
    public struct SpawnPrefabData : IBufferElementData
    {
        public int ID;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}