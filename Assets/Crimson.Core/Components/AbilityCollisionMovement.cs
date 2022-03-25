using Crimson.Core.Common;
using Sirenix.OdinInspector;
using System;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
	public enum CollisionMovementReaction
	{
		Ignore = 0,
		Stop = 1,
		Bounce = 2
	}

	public struct BounceData : IComponentData
	{
		public float InputMultiplier;
		public bool Force2DBounce;
	}

	public struct StopMovementData : IComponentData
	{
	}

	public struct StopRotationData : IComponentData
	{
	}

	[HideMonoScript]
	public class AbilityCollisionMovement : MonoBehaviour, IActorAbility
	{
		public CollisionMovementSettings collisionMovementSettings;
		private Entity _entity;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;
		}

		public void Execute()
		{
			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			switch (collisionMovementSettings.reaction)
			{
				case CollisionMovementReaction.Ignore:
					break;

				case CollisionMovementReaction.Stop:
					dstManager.AddComponent<StopMovementData>(_entity);
					if (collisionMovementSettings.alsoStopRotation) dstManager.AddComponent<StopRotationData>(_entity);
					break;

				case CollisionMovementReaction.Bounce:
					dstManager.AddComponentData(_entity, new BounceData
					{
						InputMultiplier = collisionMovementSettings.inputMultiplier,
						Force2DBounce = collisionMovementSettings.force2DBounce
					}) ;
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	[Serializable]
	public class CollisionMovementSettings
	{
		[ShowIf("reaction", CollisionMovementReaction.Stop)]
		public bool alsoStopRotation = true;

		[ShowIf("reaction", CollisionMovementReaction.Bounce)]
		public float inputMultiplier = 0.75f;

		[ShowIf("reaction", CollisionMovementReaction.Bounce)]
		public bool force2DBounce = true;

		[EnumToggleButtons]
		public CollisionMovementReaction reaction;
	}
}