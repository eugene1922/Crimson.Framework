using Crimson.Core.Components;
using Crimson.Core.Utils.LowLevel;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Systems
{
	[UpdateInGroup(typeof(FixedUpdateGroup))]
	public class ExplosionsSystem : ComponentSystem
	{
		private EntityQuery _forceQuery;

		protected override void OnCreate()
		{
			_forceQuery = GetEntityQuery(
				ComponentType.ReadOnly<AbilityActorPlayer>(),
				ComponentType.ReadOnly<ExplosionForceData>(),
				ComponentType.ReadOnly<Rigidbody>());
		}

		protected override void OnUpdate()
		{
			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			Entities.With(_forceQuery).ForEach(
				(Entity entity, Rigidbody rigidBody, ref ExplosionForceData data) =>
				{
					if (rigidBody == null)
					{
						return;
					}

					rigidBody.AddExplosionForce(data.Force, data.SourcePosition, data.Radius);
					dstManager.RemoveComponent<ExplosionForceData>(entity);
					dstManager.AddComponent<StopMovementData>(entity);
				}
			);
		}
	}
}