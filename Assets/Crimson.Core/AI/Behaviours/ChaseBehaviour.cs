using Assets.Crimson.Core.AI;
using Assets.Crimson.Core.AI.GeneralParams;
using Assets.Crimson.Core.Common.Filters;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Crimson.Core.AI
{
	[Serializable, HideMonoScript]
	public class ChaseBehaviour : MonoBehaviour, IActorAbility, IAIBehaviour
	{
		public BasePriority Priority = new BasePriority
		{
			Value = 1
		};

		public CurvePriority CurvePriority = new CurvePriority(0)
		{
			XAxisTooltip = "Target priority based on distance to it"
		};

		public EvaluationMode EvaluationMode;

		public TagFilter TagFilter;

		public IActor Actor { get; set; }

		private const float FINISH_CHASE_DISTSQ = 5f;
		private const float PRIORITY_MULTIPLIER = 0.5f;

		private Transform _target = null;
		private Transform _transform = null;
		private NavMeshPath _path;

		private int _currentWaypoint = 0;

		public float Evaluate(Entity entity, AbilityAIInput ai, List<Transform> targets)
		{
			_target = null;
			_transform = Actor?.GameObject.transform;

			if (_transform == null)
			{
				return 0f;
			}

			var filteredTargets = targets.Where(t => TagFilter.Filter(t) && t != _transform).ToList();
			if (filteredTargets.Count == 0)
			{
				return 0f;
			}

			if (filteredTargets.Count == 1)
			{
				_target = filteredTargets.First();
				return math.distancesq(_transform.position, _target.position) < FINISH_CHASE_DISTSQ
					? 0f :
					Priority.Value * PRIORITY_MULTIPLIER;
			}

			var sampleScale = CurvePriority.SampleScale;

			switch (EvaluationMode)
			{
				case EvaluationMode.Random:
					var priorities = new List<MinMaxTarget>();

					var priorityCache = 0f;

					foreach (var target in filteredTargets)
					{
						var priority = CalculatePriorityFor(target.position);

						priorities.Add(new MinMaxTarget
						{
							Min = priorityCache,
							Max = priority + priorityCache,
							Target = target
						});

						priorityCache += priority;
					}

					var randomNumber = Random.Range(0f, priorityCache);

					_target = priorities.Find(t => t.Min < randomNumber && t.Max >= randomNumber).Target;
					break;

				default: // ReSharper disable once RedundantCaseLabel
				case EvaluationMode.Strict:
					var orderedTargets = filteredTargets
						.OrderBy(t => CalculatePriorityFor(t.position))
						.ToList();

					_target = orderedTargets.Last();
					break;
			}

			return math.distancesq(_transform.position, _target.position) < FINISH_CHASE_DISTSQ ?
				0f :
				Priority.Value * PRIORITY_MULTIPLIER;
		}

		private float CalculatePriorityFor(Vector3 position)
		{
			var distance = math.distance(_transform.position, position);
			return CurvePriority.Evaluate(distance);
		}

		public bool SetUp(Entity entity, EntityManager dstManager)
		{
			if (_path == null)
			{
				_path = new NavMeshPath();
			}
			_path.ClearCorners();

			if (_target == null || _transform == null)
			{
				return false;
			}

			_currentWaypoint = 1;
			var result = NavMesh.CalculatePath(_transform.position, _target.position, NavMesh.AllAreas, _path);

			return result;
		}

		public bool Behave(Entity entity, EntityManager dstManager, ref PlayerInputData inputData)
		{
			if (_path.status == NavMeshPathStatus.PathInvalid)
			{
				return false;
			}

			var distSq = math.distancesq(_transform.position, _path.corners[_currentWaypoint]);

			if (distSq <= Constants.WAYPOINT_SQDIST_THRESH)
			{
				_currentWaypoint++;
			}

			if (_currentWaypoint >= _path.corners.Length)
			{
				inputData.Move = float2.zero;
				return false;
			}

			var dir = _path.corners[_currentWaypoint] - _transform.position;

			inputData.Move = math.normalize(new float2(dir.x, dir.z));

			return true;
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Execute()
		{
		}
	}
}