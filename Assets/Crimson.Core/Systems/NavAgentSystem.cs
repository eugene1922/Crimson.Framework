using Assets.Crimson.Core.Components;
using Assets.Crimson.Core.Utils;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine.AI;

namespace Assets.Crimson.Core.Systems
{
	public class NavAgentSystem : ComponentSystem
	{
		private EntityQuery _syncMoveQuery;

		protected override void OnCreate()
		{
			_syncMoveQuery = GetEntityQuery(
				ComponentType.ReadWrite<PlayerInputData>(),
				ComponentType.ReadOnly<NavMeshAgent>(),
				ComponentType.ReadOnly<AnimatorProxy>()
				);
		}

		protected override void OnUpdate()
		{
			Entities.With(_syncMoveQuery).ForEach(
				(ref PlayerInputData data, NavMeshAgent agent, AnimatorProxy proxy) =>
				{
					var move = agent.GetMoveDirection();
					data.Move = move;
				});
		}
	}
}