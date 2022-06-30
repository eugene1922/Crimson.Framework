using Assets.Crimson.Core.AI.GeneralParams;
using Assets.Crimson.Core.Utils;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Crimson.Core.Components
{
	public class AbilityNavMeshPoints : MonoBehaviour, IActorAbility
	{
		public NavMeshAgent Agent;

		public DistanceLimitation DistanceLimitation;

		public int MaxPoints = 16;
		private Vector3[] _positions;
		public float DebugSphereRadius = 1.5f;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		[Button]
		public void Execute()
		{
			_positions = new Vector3[MaxPoints];
			_positions = NavMeshUtils.CalculatePositionsOnCircle(transform.position, DistanceLimitation.MaxDistance, MaxPoints);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			if (_positions == null)
			{
				return;
			}
			for (var i = 0; i < _positions.Length; i++)
			{
				Gizmos.DrawSphere(_positions[i], DebugSphereRadius);
			}
		}
	}
}