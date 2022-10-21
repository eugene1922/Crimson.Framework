using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityTargetDash : MonoBehaviour, IActorAbility
	{
		public AbilityFindTargetActor AbilityTarget;
		public float PositionThreshold = 1;

		[ValueDropdown(nameof(_modes))]
		public string Mode;

		public float Value = 2;

		private Entity _entity;
		private EntityManager _entityManager;
		public IActor Actor { get; set; }

		private string[] _modes = new string[]
			{
				"Duration",
				"Velocity"
			};

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		public void Execute()
		{
			var target = AbilityTarget.Target.transform;
			var direction = target.position - transform.position;
			var lookToTarget = Quaternion.LookRotation(direction);
			transform.rotation = lookToTarget;
			var moveData = new MoveData()
			{
				EndPosition = target.position,
				PositionThreshold = PositionThreshold
			};

			switch (Mode)
			{
				case "Duration":
					moveData.Velocity = direction.magnitude / Value;
					break;

				case "Velocity":
					moveData.Velocity = Value;
					break;

				default:
					break;
			}
			_entityManager.AddComponentData(_entity, moveData);
		}
	}
}