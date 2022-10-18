using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Perks
{
	public class AbilityDeadlyJump : MonoBehaviour, IActorAbility
	{
		public AbilityFindTargetActor AbilityTarget;

		[MinValue(0.01)]
		public float Duration;

		[ValidateInput("MustBeAbility", "Ability MonoBehaviours must derive from IActorAbility!")]
		[PropertyOrder(1)] public List<MonoBehaviour> FXActions;

		public float PositionThreshold = 1;
		private IEnumerable<IActorAbility> _actions;
		private Entity _entity;
		private EntityManager _entityManager;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			_actions = FXActions.Cast<IActorAbility>();
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
				PositionThreshold = PositionThreshold,
				Velocity = direction.magnitude / Duration
			};
			_entityManager.AddComponentData(_entity, moveData);
			foreach (var ability in _actions)
			{
				ability.Execute();
			}
		}

		private bool MustBeAbility(List<MonoBehaviour> a)
		{
			return !a.Exists(t => !(t is IActorAbility)) || a.Count == 0;
		}
	}
}