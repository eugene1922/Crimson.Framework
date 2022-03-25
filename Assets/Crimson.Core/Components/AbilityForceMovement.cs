using Crimson.Core.Common;
using Sirenix.OdinInspector;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Components
{
	[HideMonoScript]
	public class AbilityForceMovement : MonoBehaviour, IActorAbility
	{
		public IActor Actor { get; set; }

		[InfoBox("To set movement speed and dynamics, use AbilityMovement component")]
		[EnumToggleButtons] public MoveDirection moveDirection;

		[ShowIf("moveDirection", MoveDirection.UseDirection)]
		public Vector3 forwardVector;

		[ShowIf("moveDirection", MoveDirection.UseDirection)]
		public bool compensateSpawnerRotation = true;

		[ShowIf("moveDirection", MoveDirection.SpawnerForward)]
		public Vector3 OffsetDirection = Vector3.zero;

		public Transform Spawner => Actor.Spawner.GameObject.transform;

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;

			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			dstManager.AddComponentData(entity, new MoveByInputData());
			dstManager.AddComponentData(entity, new ActorForceMovementData
			{
				MoveDirection = moveDirection,
				ForwardVector = forwardVector,
				OffsetDirection = OffsetDirection,
				CompensateSpawnerRotation = compensateSpawnerRotation,
				stopGuiding = false
			});
		}

		public void Execute()
		{
		}
	}

	public struct ActorForceMovementData : IComponentData
	{
		public MoveDirection MoveDirection;
		public float3 ForwardVector;
		public bool CompensateSpawnerRotation;
		public bool stopGuiding;
		public float3 OffsetDirection;
	}

	public enum MoveDirection
	{
		SpawnerForward = 0,
		UseDirection = 1,
		GuidedBySpawner = 2,
		SelfForward = 3
	}
}