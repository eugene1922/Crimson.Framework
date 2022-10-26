using Assets.Crimson.Core.Common;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Crimson.Core.Systems
{
	public class AdditionalMovementSystem : ComponentSystem
	{
		private EntityQuery _moveAgentQuery;
		private EntityQuery _moveTransformQuery;

		protected override void OnCreate()
		{
			_moveTransformQuery = GetEntityQuery(
				ComponentType.ReadOnly<Transform>(),
				ComponentType.Exclude<NavMeshAgent>(),
				ComponentType.ReadWrite<MoveData>()
				);

			_moveAgentQuery = GetEntityQuery(
				ComponentType.ReadOnly<Transform>(),
				ComponentType.ReadOnly<NavMeshAgent>(),
				ComponentType.ReadWrite<MoveData>()
				);
		}

		protected override void OnUpdate()
		{
			Entities.With(_moveTransformQuery).ForEach(
				(Entity entity, Transform transform, ref MoveData moveData) =>
				{
					var direction = (Vector3)moveData.EndPosition - transform.position;
					var results = new RaycastHit[1];
					if (Physics.RaycastNonAlloc(transform.position, direction, results, moveData.PositionThreshold) > 0
					 || direction.magnitude <= moveData.PositionThreshold)
					{
						EntityManager.RemoveComponent<MoveData>(entity);
						return;
					}
					transform.position += direction.normalized * moveData.Velocity * UnityEngine.Time.deltaTime;
				});

			Entities.With(_moveAgentQuery).ForEach(
				(Entity entity, NavMeshAgent agent, ref MoveData moveData) =>
				{
					var direction = (Vector3)moveData.EndPosition - agent.transform.position;
					var results = new RaycastHit[1];
					if (Physics.RaycastNonAlloc(agent.transform.position, direction, results, moveData.PositionThreshold) > 0
					 || direction.magnitude <= moveData.PositionThreshold)
					{
						EntityManager.RemoveComponent<MoveData>(entity);
						return;
					}
					agent.Move(direction.normalized * moveData.Velocity * UnityEngine.Time.deltaTime);
				});
		}
	}
}