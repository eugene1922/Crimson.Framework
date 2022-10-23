using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components;
using Crimson.Core.Common;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Systems
{
	public class TeleportSystem : ComponentSystem
	{
		private EntityQuery _teleportTargetQuery;

		protected override void OnCreate()
		{
			_teleportTargetQuery = GetEntityQuery(
				ComponentType.ReadOnly<TeleportData>(),
				ComponentType.ReadOnly<AbilityTargetTeleport>()
				);
		}

		protected override void OnUpdate()
		{
			Entities.With(_teleportTargetQuery).ForEach(
				(Entity entity, ref TeleportData teleportData, AbilityTargetTeleport teleportAbility, Transform transform) =>
				{
					teleportAbility.SpawnFX(teleportAbility.PreFX);
					var source = teleportAbility;
					Transform targetTransform = null;
					var distance = float.MaxValue;

					Entities.WithAll<Actor>().ForEach(
						(Actor actor) =>
						{
							var distanceToActor = Vector3.Distance(actor.transform.position, source.transform.position);
							if (teleportAbility.AbilityTarget.TagFilter.Filter((IActor)actor) && distanceToActor < distance)
							{
								targetTransform = actor.transform;
								distance = distanceToActor;
							}
						});

					var direction = targetTransform.position - transform.position;
					var thresholdVector = direction.normalized * teleportData.PositionThreshold;
					var position = targetTransform.position - thresholdVector;
					transform.position = position;
					EntityManager.RemoveComponent<TeleportData>(entity);
					teleportAbility.SpawnFX(teleportAbility.PostFX);
				});
		}
	}
}