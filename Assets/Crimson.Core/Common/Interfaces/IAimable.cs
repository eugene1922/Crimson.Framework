using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Common
{
	public interface IAimable
	{
		bool ActionExecutionAllowed { get; set; }
		AimingAnimationProperties AimingAnimProperties { get; set; }
		bool AimingAvailable { get; set; }
		AimingProperties AimingProperties { get; set; }
		bool DeactivateAimingOnCooldown { get; set; }
		bool OnHoldAttackActive { get; set; }
		GameObject SpawnedAimingPrefab { get; set; }

		void EvaluateAim(Vector2 pos);

		void EvaluateAimBySelectedType(Vector2 pos);

		void ResetAiming();
	}

	public struct ActorEvaluateAimingAnimData : IComponentData
	{
		public bool AimingActive;
	}

	public struct AimingAnimProperties : IComponentData
	{
		public int AnimHash;
	}

	public struct AimingData : IComponentData
	{
		public float3 Direction;
	}
}