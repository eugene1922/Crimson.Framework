using System;
using UnityEngine;

namespace Crimson.Core.Serialization
{
    [Serializable]
    public sealed class PrefabItem
    {
        public GameObject asset;

        [Sirenix.OdinInspector.ReadOnly]
        public int id;

        public PrefabItem(GameObject asset)
        {
            id = Guid.NewGuid().GetHashCode();
            this.asset = asset;
        }

        [HideInInspector]
        public string name => asset.name;
    }
}