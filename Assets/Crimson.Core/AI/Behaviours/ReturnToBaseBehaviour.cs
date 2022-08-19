using Assets.Crimson.Core.AI.GeneralParams;
using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.AI;
using Crimson.Core.Common;
using Crimson.Core.Components;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Crimson.Core.AI.Behaviours
{
	public class ReturnToBaseBehaviour : MonoBehaviour, IActorAbility, IAIBehaviour
	{
		public BasePriority Priority = new BasePriority
		{
			Value = 1
		};

		public EvaluatedCurve CurvePriority = new EvaluatedCurve(0)
		{
			XAxisTooltip = "Target priority based on distance to it"
		};

		private const float _breakDistance = 1;

		private BasePointData _basePoint;
		private EntityManager _entityManager;
		private AIPathControl _path;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_path = new AIPathControl(transform);
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		public bool Behave(Entity entity, EntityManager dstManager, ref PlayerInputData inputData)
		{
			_path.SetTarget(_basePoint.Position);
			inputData.Move = _path.MoveDirection;
			if (_path.HasArrived)
			{
				var movementData = _entityManager.GetComponentData<ActorMovementData>(entity);
				movementData.ExternalMultiplier = 1;
				_entityManager.SetComponentData(entity, movementData);
			}
			return !_path.HasArrived;
		}

		public float Evaluate(Entity entity, AbilityAIInput ai, List<Transform> targets)
		{
			_basePoint = _entityManager.GetComponentData<BasePointData>(entity);
			var hasTag = _entityManager.HasComponent<AggressiveAITag>(entity);
			var distance = math.distance(transform.position, _basePoint.Position);
			return !hasTag && distance > _breakDistance ?
				CurvePriority.Evaluate(distance)
				: 0f;
		}

		public void Execute()
		{
		}

		public bool SetUp(Entity entity, EntityManager entityManager)
		{
			var movementData = entityManager.GetComponentData<ActorMovementData>(entity);
			movementData.ExternalMultiplier = 2;
			entityManager.SetComponentData(entity, movementData);
			entityManager.AddComponentData(entity, new FullHealPercentBuff(10));
			return true;
		}
	}
}