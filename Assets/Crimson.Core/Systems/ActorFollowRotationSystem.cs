using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Systems
{
	public class ActorFollowRotationSystem : ComponentSystem
	{
		private EntityQuery _lookQuery;
		private EntityQuery _query;

		protected override void OnCreate()
		{
			_query = GetEntityQuery(
				ComponentType.ReadOnly<Transform>(),
				ComponentType.ReadOnly<ActorFollowRotationData>(),
				ComponentType.ReadOnly<RotateDirectlyData>(),
				ComponentType.Exclude<ActorNoFollowTargetRotationData>(),
				ComponentType.Exclude<StopRotationData>());

			_lookQuery = GetEntityQuery(
				ComponentType.ReadOnly<Transform>(),
				ComponentType.ReadOnly<FollowLookRotationData>(),
				ComponentType.ReadOnly<PlayerInputData>(),
				ComponentType.Exclude<StopRotationData>());
		}

		protected override void OnUpdate()
		{
			Entities.With(_query).ForEach(
				(Entity entity, AbilityFollowRotation follow, ref RotateDirectlyData rotation,
					ref ActorFollowRotationData data) =>
				{
					if (follow.target == null)
					{
						PostUpdateCommands.AddComponent<ActorNoFollowTargetRotationData>(entity);
						if (follow.hideIfNoTarget) follow.gameObject.SetActive(false);
						return;
					}

					if (follow.hideIfNoTarget)
					{
						follow.gameObject.SetActive(true);
					}

					float3 targetEuler = follow.target.rotation.eulerAngles;

					var newRotation = follow.retainRotationOffset ? targetEuler - data.Origin : targetEuler;
					rotation.Rotation = newRotation;
				}
			);

			//Entities.With(_lookQuery).ForEach(
			//	(Transform transform, ref PlayerInputData input, ref FollowLookRotationData lookRotationTag) =>
			//	{
			//		var mouse = (Vector2)input.Mouse;
			//		var position = (Vector2)Camera.main.WorldToScreenPoint(transform.position + (Vector3)lookRotationTag.Offset);
			//		var direction = mouse - position;
			//		var angle = Mathf.Atan2(direction.y / Mathf.Sin(input.DeclinationAngle * Mathf.Deg2Rad), direction.x) * Mathf.Rad2Deg - input.CompensateAngle;

			//		transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.up);
			//		input.Look = direction.normalized;
			//	});
		}
	}
}