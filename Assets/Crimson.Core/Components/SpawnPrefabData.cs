using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
    public struct SpawnPrefabData : IBufferElementData
    {
        public ushort ID;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}