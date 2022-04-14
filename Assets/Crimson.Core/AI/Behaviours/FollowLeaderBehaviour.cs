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

namespace Crimson.Core.AI
{
	[Serializable, HideMonoScript]
	public class FollowLeaderBehaviour : MonoBehaviour, IActorAbility, IAIBehaviour
	{
		public BasePriority BasePriority = new BasePriority
		{
			Value = 1
		};

		public CurvePriority CurvePriority = new CurvePriority(0);
		public TagFilter TagFilter;

		private const float FINISH_BEHAVIOUR_DISTSQ = 20f;
		private NavMeshPath _path;
		private int _currentWaypoint = 0;
		private Transform _target = null;
		private Transform _transform = null;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public bool Behave(Entity entity, EntityManager dstManager, ref PlayerInputData inputData)
		{
			if (_path.status == NavMeshPathStatus.PathInvalid || _transform == null)
			{
				return false;
			}

			var distSq = math.distancesq(_transform.position, _path.corners[_currentWaypoint]);

			if (distSq <= Constants.WAYPOINT_SQDIST_THRESH)
			{
				_currentWaypoint++;
			}

			if ((_currentWaypoint == _path.corners.Length - 1 && distSq < FINISH_BEHAVIOUR_DISTSQ) || _currentWaypoint >= _path.corners.Length)
			{
				inputData.Move = float2.zero;
				return false;
			}

			var dir = math.normalize(_path.corners[_currentWaypoint] - _transform.position);

			inputData.Move = new float2(dir.x, dir.z);

			return true;
		}

		public float Evaluate(Entity entity, AbilityAIInput ai, List<Transform> targets)
		{
			_target = null;
			_transform = Actor.GameObject.transform;
			_target = targets.FirstOrDefault(t => TagFilter.Filter(t) && t != _transform);

			if (_target == null)
			{
				return 0;
			}

			var distance = math.distance(_transform.position, _target.position);
			var result = CurvePriority.Evaluate(distance) * BasePriority.Value;

			return result;
		}

		public void Execute()
		{
		}

		public bool SetUp(Entity entity, EntityManager dstManager)
		{
			if (_path == null)
			{
				_path = new NavMeshPath();
			}
			_path.ClearCorners();

			_currentWaypoint = 1;

			var target = _target.position;

			var result = NavMesh.CalculatePath(_transform.position, target, NavMesh.AllAreas, _path);

			return result;
		}
	}
}