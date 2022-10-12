using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components;
using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Crimson.Core.Systems
{
	public class AimAssistSystem : ComponentSystem
	{
		private EntityQuery _aimQuery;

		protected override void OnCreate()
		{
			_aimQuery = GetEntityQuery(
				ComponentType.ReadOnly<AbilityAimAssist>(),
				ComponentType.ReadWrite<AimData>(),
				ComponentType.ReadOnly<PlayerInputData>()
				);
		}

		protected override void OnUpdate()
		{
			Entities.With(_aimQuery).ForEach(
				(Entity entity, AbilityAimAssist ability, ref AimData aimData, ref PlayerInputData inputData) =>
				{
					var aimPosition = ability.AimPositionByInput(inputData);
					var targetPosition = aimPosition;
					var aimDirection = aimPosition - ability.transform.position;
					var minimalAimDistance = float.MaxValue;
					var minimalSourceDistance = ability.AimRange;
					var minimalAngle = 90f;
					Entities.WithAll<EnemyData>().ForEach(
						(Actor actor, ref EnemyData enemyData) =>
						{
							var enemyPosition = actor.transform.position + (Vector3)enemyData.Offset;
							if (Vector3.Distance(enemyPosition, ability.transform.position) <= ability.AimRange)
							{
								var distanceToAim = math.distance(enemyPosition, aimPosition);
								var sourceDistance = math.distance(enemyPosition, ability.transform.position);
								var targetDirection = enemyPosition - ability.transform.position;
								var angle = Vector3.Angle(targetDirection, aimDirection);
								if (angle < minimalAngle
									&& ability.LockRange > distanceToAim
									&& sourceDistance < minimalSourceDistance)
								{
									targetPosition = enemyPosition;
									minimalAimDistance = distanceToAim;
									minimalSourceDistance = sourceDistance;
									minimalAngle = angle;
								}
							}
						});
					aimData.LockedPosition = targetPosition;
					EntityManager.SetComponentData(entity, aimData);
				});
		}
	}
}