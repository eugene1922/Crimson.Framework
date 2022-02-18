using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils.LowLevel;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Systems
{
	[UpdateInGroup(typeof(FixedUpdateGroup))]
	public class ActorRotationDirectlySystemRigidboby : ComponentSystem
	{
		private EntityQuery _query;

		protected override void OnCreate()
		{
			_query = GetEntityQuery(
				ComponentType.ReadOnly<Transform>(),
				ComponentType.ReadOnly<RotateDirectlyData>(),
				ComponentType.Exclude<ActorNoFollowTargetRotationData>(),
				ComponentType.ReadOnly<Rigidbody>(),
				ComponentType.Exclude<StopRotationData>());
		}

		protected override void OnUpdate()
		{
			Entities.With(_query).ForEach(
				(Entity entity, Rigidbody transform, ref RotateDirectlyData rotation) =>
				{
					if (transform == null)
					{
						return;
					}

					float3 currentRotation = transform.rotation.eulerAngles;
					var newRotation = currentRotation;

					for (var i = 0; i < 3; i++)
					{
						if (rotation.Constraints[i])
						{
							newRotation[i] = rotation.Rotation[i];
						}
					}

					transform.rotation = Quaternion.Euler(newRotation);
				});
		}
	}
}