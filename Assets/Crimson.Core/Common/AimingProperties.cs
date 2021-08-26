using System;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crimson.Core.Common
{
    [Serializable]
    public struct AimingProperties
    {
        [Space] [EnumToggleButtons] public EvaluateActionOptions evaluateActionOptions;
        [Space][EnumToggleButtons] public AimingType aimingType;
        
        public GameObject aimingAreaPrefab;
        public GameObject sightPrefab;
        public GameObject circlePrefab;

        [ShowIf("@aimingType == AimingType.SightControl")]
        public float sightDistance;
    }
}