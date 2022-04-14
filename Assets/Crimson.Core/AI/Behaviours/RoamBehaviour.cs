using Assets.Crimson.Core.AI.GeneralParams;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Crimson.Core.AI
{
	[Serializable, HideMonoScript]
	public class RoamBehaviour : MonoBehaviour, IActorAbility, IAIBehaviour
	{
		public BasePriority Priority = new BasePriority
		{
			Value = 1
		};

		public IActor Actor { get; set; }

		private const float FINISH_ROAM_DISTSQ = 2f;
		private const float PRIORITY_MULTIPLIER = 0.5f;

		private Transform _transform = null;
		private NavMeshPath _path;

		private int _currentWaypoint = 0;

		public float Evaluate(Entity entity, AbilityAIInput ai, List<Transform> targets)
		{
			_transform = Actor.GameObject.transform;

			return Random.value * Priority.Value * PRIORITY_MULTIPLIER;
		}

		public bool SetUp(Entity entity, EntityManager dstManager)
		{
			Vector3 target;
			float distSq;

			if (_path == null)
			{
				_path = new NavMeshPath();
			}
			_path.ClearCorners();

			_currentWaypoint = 1;

			do
			{
				target = NavMeshRandomPointUtil.GetRandomLocation();
				distSq = math.distancesq(_transform.position, target);
			} while (distSq < FINISH_ROAM_DISTSQ);

			var result = NavMesh.CalculatePath(_transform.position, target, NavMesh.AllAreas, _path);

			return result;
		}

		public bool Behave(Entity entity, EntityManager dstManager, ref PlayerInputData inputData)
		{
			if (_path == null
				|| _path.status == NavMeshPathStatus.PathInvalid
				|| _transform == null)
			{
				return false;
			}

			var distSq = math.distancesq(_transform.position, _path.corners[_currentWaypoint]);

			if (distSq <= Constants.WAYPOINT_SQDIST_THRESH)
			{
				_currentWaypoint++;
			}

			if ((_currentWaypoint == _path.corners.Length - 1 && distSq < FINISH_ROAM_DISTSQ) || _currentWaypoint >= _path.corners.Length)
			{
				inputData.Move = float2.zero;
				return false;
			}

			var dir = math.normalize(_path.corners[_currentWaypoint] - _transform.position);

			inputData.Move = new float2(dir.x, dir.z);

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