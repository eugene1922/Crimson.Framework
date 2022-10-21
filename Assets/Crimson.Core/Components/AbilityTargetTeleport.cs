using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityTargetTeleport : MonoBehaviour, IActorAbility
	{
		public AbilityFindTargetActor AbilityTarget;
		public float PositionThreshold = 1;
		private Entity _entity;
		private EntityManager _entityManager;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entity = entity;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		public void Execute()
		{
			var teleportData = new TeleportData()
			{
				PositionThreshold = PositionThreshold
			};
			_entityManager.AddComponentData(_entity, teleportData);
		}
	}
}