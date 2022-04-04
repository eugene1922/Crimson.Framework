using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Common.Magnets;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Magnets
{
	public class AbilityMagnetSnap : MonoBehaviour, IActorAbilityTarget
	{
		public float _snapThreshold = .5f;
		private Entity _entity;

		private EntityManager _entityManager;
		public IActor AbilityOwnerActor { get; set; }
		public IActor Actor { get; set; }
		public IActor TargetActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;
			AbilityOwnerActor = actor;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		public void Execute()
		{
			if (TargetActor == null)
			{
				return;
			}

			var hasMagnetAbility = _entityManager.HasComponent<AbilityMagnet>(TargetActor.ActorEntity);
			var isUsing = _entityManager.HasComponent<MagnetRigidbodyData>(TargetActor.ActorEntity);
			if (!hasMagnetAbility || isUsing)
			{
				return;
			}

			if (!_entityManager.HasComponent<MagnetSnapData>(TargetActor.ActorEntity))
			{
				_entityManager.AddComponentData(TargetActor.ActorEntity, new MagnetSnapData(_entity, _snapThreshold));
			}
		}
	}
}