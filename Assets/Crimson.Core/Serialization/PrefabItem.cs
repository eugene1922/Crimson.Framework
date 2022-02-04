using System;
using UnityEngine;

namespace Crimson.Core.Serialization
{
    [Serializable]
    public sealed class PrefabItem : ISerializationCallbackReceiver
    {
        [Sirenix.OdinInspector.ReadOnly]
        public ushort id;
        public GameObject asset;

        [HideInInspector]
        public string name;

        public PrefabItem(GameObject asset)
        {
            this.asset = asset;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (asset == null)
            {
                return;
            }

            id = HashUtils.GetAssetHash(asset);
            name = asset.name;
#endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() { }

    }
}