using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components;
using Assets.Crimson.Core.Components.Tags;
using Assets.Crimson.Core.Components.Targets;
using Codice.CM.Common.Merge;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.Utilities;
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
					var sourcePosition = ability.transform.position + ability.Offset;
					var aimPosition = ability.AimPositionByInput(inputData);
					aimData.RealPosition = aimPosition;
					var minimalSourceDistance = ability.AimRange;
					var targetData = new EnemyTargetData
					{
						Position = aimPosition
					};
					var aimDirection = sourcePosition - aimPosition;
					var ray = new Ray(sourcePosition, aimDirection);

					Entities.With(_visibleEnemy).ForEach(
						(Actor actor, ref EnemyData enemyData) =>
						{
							var enemyOffset = (Vector3)enemyData.Offset;
							var enemyPosition = actor.transform.position + enemyOffset;
							if (Vector3.Distance(enemyPosition, sourcePosition) <= ability.AimRange)
							{
								var enemyDirection = sourcePosition - enemyPosition;
								if (Vector3.Dot(enemyDirection, aimDirection) < 1)
								{
									return;
								}

								if (enemyDirection.magnitude > aimDirection.magnitude)
								{
									if (!InRange(enemyData.LockRange, aimPosition - enemyPosition))
									{
										return;
									}
								}
								else
								{
									var enemyProjectDirection = Vector3.Project(enemyDirection, aimDirection);
									if (!InRange(enemyData.LockRange, enemyProjectDirection - enemyDirection))
									{
										return;
									}
								}
								var sourceDistance = math.distance(enemyPosition, sourcePosition);
								if (sourceDistance < minimalSourceDistance)
								{
									targetData.Position = enemyPosition;
									targetData.Rotation = actor.transform.rotation;
									minimalSourceDistance = sourceDistance;
									targetData.Entity = actor.ActorEntity;
								}
							}
						});

					aimData.Target = targetData.Entity;
					if (EntityManager.HasComponent<EnemyTargetData>(entity))
					{
						EntityManager.SetComponentData(entity, targetData);
					}
					else
					{
						EntityManager.AddComponentData(entity, targetData);
					}

					aimData.LockedPosition = targetData.Position;
					EntityManager.SetComponentData(entity, aimData);
				});
		}

		private bool InRange(Vector3 range, Vector3 target)
		{
			var result = true;
			var difference = range - target.Abs();
			result &= difference.x >= 0;
			result &= difference.z >= 0;
			if (result)
			{
				result |= difference.y >= 0;
			}

			return result;
		}
	}
}