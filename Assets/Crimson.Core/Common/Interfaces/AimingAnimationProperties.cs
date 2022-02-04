using Sirenix.OdinInspector;
using System;

namespace Crimson.Core.Common
{
    [Serializable]
    public class AimingAnimationProperties
    {
        [ToggleLeft] public bool HasActorAimingAnimation = false;

        [ShowIf("@HasActorAimingAnimation")]
        public string ActorAimingAnimationName;
    }
}