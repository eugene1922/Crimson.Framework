using Crimson.Core.Common;
using Crimson.Core.Enums;
using Sirenix.OdinInspector;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Components
{
	public struct ActorFollowMovementData : IComponentData
	{
		public float3 Origin;
	}

	public struct ActorNoFollowTargetMovementData : IComponentData
	{
	}

	[HideMonoScript]
	public class AbilityFollowMovement : MonoBehaviour, IActorAbility
	{
		[ShowIf("followMovementType", FollowType.UseMovementComponent)]
		public bool continousFollow = true;

		public FindTargetProperties findTargetProperties;
		[Space] [EnumToggleButtons] public FollowType followMovementType;
		public bool hideIfNoTarget = false;

		[ShowIf("followMovementType", FollowType.Simple)]
		[InfoBox("Speed of 0 results in unconstrained speed")]
		[MinValue(0)]
		public float movementSpeed = 0;

		[ShowIf("followMovementType", FollowType.Simple)]
		public bool retainOffset = false;

		public IActor Actor { get; set; }
		public Transform Target { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;

			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			dstManager.AddComponentData(entity, new ActorFollowMovementData());

			dstManager.AddComponentData(entity, new ActorNoFollowTargetMovementData());

			if (followMovementType == FollowType.UseMovementComponent)
			{
				dstManager.AddComponentData(entity, new MoveByInputData());
			}
			else
			{
				dstManager.AddComponentData(entity, new MoveDirectlyData
				{
					Speed = movementSpeed
				});
			}

			if (hideIfNoTarget)
			{
				gameObject.SetActive(false);
			}
		}

		public void Execute()
		{
		}
	}
}