using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Crimson.Core.Common
{
	[Serializable]
	public struct AimingProperties
	{
		[Space] [EnumToggleButtons] public EvaluateActionOptions evaluateActionOptions;
		[Space] [EnumToggleButtons] public AimingType aimingType;

		[ShowIf("@aimingType == AimingType.AimingArea")] public GameObject aimingAreaPrefab;
		[ShowIf("@aimingType == AimingType.SightControl")] public GameObject sightPrefab;
		[ShowIf("@aimingType == AimingType.Circle")] public GameObject circlePrefab;

		[ShowIf("@aimingType == AimingType.SightControl")]
		public float sightDistance;

		[ShowIf("@aimingType == AimingType.SightControl")]
		public bool reverseControls;
	}
}