using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Enums;
using Crimson.Core.Utils;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityMovementPlatform : MonoBehaviour, IActorAbility
	{
		public Collider ColliderZone;
		private Entity _entity;
		private EntityManager _entityManager;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entity = entity;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			_entityManager.AddComponentData(_entity, new MovingPlatformData(transform));
			InitCollider(ColliderZone);
		}

		public void Execute()
		{
		}

		private void InitCollider(Collider collider)
		{
			float3 position = gameObject.transform.position;
			switch (collider)
			{
				case SphereCollider sphere:
					sphere.ToWorldSpaceSphere(out var sphereCenter, out var sphereRadius);
					_entityManager.AddComponentData(_entity, new ActorColliderData
					{
						ColliderType = ColliderType.Sphere,
						SphereCenter = sphereCenter - position,
						SphereRadius = sphereRadius,
						initialTakeOff = true
					});
					break;

				case CapsuleCollider capsule:
					capsule.ToWorldSpaceCapsule(out var capsuleStart, out var capsuleEnd, out var capsuleRadius);
					_entityManager.AddComponentData(_entity, new ActorColliderData
					{
						ColliderType = ColliderType.Capsule,
						CapsuleStart = capsuleStart - position,
						CapsuleEnd = capsuleEnd - position,
						CapsuleRadius = capsuleRadius,
						initialTakeOff = true
					});
					break;

				case BoxCollider box:
					box.ToWorldSpaceBox(out var boxCenter, out var boxHalfExtents, out var boxOrientation);
					_entityManager.AddComponentData(_entity, new ActorColliderData
					{
						ColliderType = ColliderType.Box,
						BoxCenter = boxCenter - position,
						BoxHalfExtents = boxHalfExtents,
						BoxOrientation = boxOrientation,
						initialTakeOff = true
					});
					break;
			}
		}
	}
}