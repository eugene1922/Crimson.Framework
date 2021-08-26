using System;
using Sirenix.OdinInspector;

namespace Crimson.Core.Common
{
    [Serializable]
    public class ActorProjectileSpawnAnimProperties
    {
        [ToggleLeft] public bool HasActorProjectileAnimation = false;

        [ShowIf("@HasActorProjectileAnimation == true")]
        public string ActorProjectileAnimationName;
    }
}