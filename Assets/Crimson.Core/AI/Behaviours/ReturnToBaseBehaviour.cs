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
	internal class ReturnToBaseBehaviour : MonoBehaviour, IActorAbility, IAIBehaviour
	{
		public EvaluatedCurve CurvePriority = new EvaluatedCurve(0)
		{
			XAxisTooltip = "Target priority based on distance to it"
		};

		public BasePriority Priority = new BasePriority
		{
			Value = 1
		};

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
			return !_path.HasArrived;
		}

		public float Evaluate(Entity entity, AbilityAIInput ai, List<Transform> targets)
		{
			_basePoint = _entityManager.GetComponentData<BasePointData>(entity);
			var hasTag = _entityManager.HasComponent<AggressiveAITag>(entity);
			return !hasTag ?
				CurvePriority.Evaluate(math.distance(transform.position, _basePoint.Position))
				: 0f;
		}

		public void Execute()
		{
		}

		public bool SetUp(Entity entity, EntityManager dstManager)
		{
			dstManager.RemoveComponent<AggressiveAITag>(entity);
			return true;
		}
	}
}