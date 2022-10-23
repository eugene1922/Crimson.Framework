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
		private EntityQuery _visibleEnemy;

		protected override void OnCreate()
		{
			_aimQuery = GetEntityQuery(
				ComponentType.ReadOnly<AbilityAimAssist>(),
				ComponentType.ReadWrite<AimData>(),
				ComponentType.ReadOnly<PlayerInputData>()
				);
			_visibleEnemy = GetEntityQuery(
				ComponentType.ReadOnly<EnemyData>(),
				ComponentType.Exclude<InvisibleTag>(),
				ComponentType.Exclude<DeadActorTag>()
				);
		}

		protected override void OnUpdate()
		{
			Entities.With(_aimQuery).ForEach(
				(Entity entity, AbilityAimAssist ability, ref AimData aimData, ref PlayerInputData inputData) =>
				{
					var aimPosition = ability.AimPositionByInput(inputData);
					aimData.RealPosition = aimPosition;
					var minimalAimDistance = float.MaxValue;
					var minimalSourceDistance = ability.AimRange;
					var minimalAngle = 90f;
					var lockedPosition = aimPosition;
					Entity target = Entity.Null;
					Entities.With(_visibleEnemy).ForEach(
						(Actor actor, ref EnemyData enemyData) =>
						{
							var enemyPosition = actor.transform.position + (Vector3)enemyData.Offset;
							if (Vector3.Distance(enemyPosition, ability.transform.position) <= ability.AimRange)
							{
								var distanceToAim = math.distance(enemyPosition, aimPosition);
								var sourceDistance = math.distance(enemyPosition, ability.transform.position);
								var targetDirection = enemyPosition - ability.transform.position;
								var angle = Vector3.Angle(targetDirection, aimPosition);
								if (angle < minimalAngle
									&& ability.LockRange > distanceToAim
									&& sourceDistance < minimalSourceDistance)
								{
									lockedPosition = enemyPosition;
									minimalAimDistance = distanceToAim;
									minimalSourceDistance = sourceDistance;
									minimalAngle = angle;
									target = actor.ActorEntity;
								}
							}
						});
					aimData.Target = target;
					aimData.LockedPosition = lockedPosition;
					EntityManager.SetComponentData(entity, aimData);
				});
		}
	}
}