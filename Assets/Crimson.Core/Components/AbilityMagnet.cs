using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityMagnet : MonoBehaviour, IActorAbility
	{
		public Entity _entity;
		public EntityManager _entityManager;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		public void Execute()
		{
		}

		internal void MagnetTo(Entity target)
		{
			if (_entityManager.HasComponent<MagnetRigidbodyData>(_entity))
			{
				return;
			}
			_entityManager.AddComponentData(_entity, new MagnetRigidbodyData(target));
		}
	}
}