using System;
using UnityEngine;

namespace Crimson.Core.Serialization
{
    [Serializable]
    public sealed class PrefabItem : ISerializationCallbackReceiver
    {
        public GameObject asset;

        [Sirenix.OdinInspector.ReadOnly]
        public int id;

        [HideInInspector]
        public string name;

        public PrefabItem(GameObject asset)
        {
            this.asset = asset;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
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
    }
}