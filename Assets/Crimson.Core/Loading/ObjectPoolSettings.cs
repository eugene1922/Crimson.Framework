using Sirenix.OdinInspector;
using System;

namespace Assets.Crimson.Core.Loading
{
    [Serializable]
    public struct ObjectPoolSettings
    {
        [ShowIf(nameof(UsePool))]
        public bool CollectionCheck;

        [ShowIf(nameof(UsePool))]
        public int InitSize;

        [ShowIf(nameof(UsePool))]
        public int MaxSize;

        public bool UsePool;
    }
}