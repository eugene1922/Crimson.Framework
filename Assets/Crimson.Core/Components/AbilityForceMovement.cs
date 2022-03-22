using Crimson.Core.Common;
using Sirenix.OdinInspector;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Components
{
	public enum MoveDirection
	{
		SpawnerForward = 0,
		UseDirection = 1,
		GuidedBySpawner = 2,
		SelfForward = 3
	}

	public struct ActorForceMovementData : IComponentData
	{
		public bool CompensateSpawnerRotation;
		public float3 ForwardVector;
		public MoveDirection MoveDirection;
		public bool stopGuiding;
	}

	[HideMonoScript]
	public class AbilityForceMovement : MonoBehaviour, IActorAbility
	{
		[ShowIf("moveDirection", MoveDirection.UseDirection)]
		public bool compensateSpawnerRotation = true;

		[ShowIf("moveDirection", MoveDirection.UseDirection)]
		public Vector3 forwardVector;

		[InfoBox("To set movement speed and dynamics, use AbilityMovement component")]
		[EnumToggleButtons] public MoveDirection moveDirection;

		public IActor Actor { get; set; }
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
				CompensateSpawnerRotation = compensateSpawnerRotation,
				stopGuiding = false
			});
		}

		public void Execute()
		{
		}

#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawLine(Vector3.zero, forwardVector);
		}

#endif
	}
}