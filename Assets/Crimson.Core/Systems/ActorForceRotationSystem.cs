using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils.LowLevel;
using Unity.Entities;
using Unity.Mathematics;

namespace Crimson.Core.Systems
{
	[UpdateInGroup(typeof(FixedUpdateGroup))]
	public class ActorForceRotationSystem : ComponentSystem
	{
		private EntityQuery _query;

		protected override void OnCreate()
		{
			_query = GetEntityQuery(
				ComponentType.ReadOnly<RotateDirectlyData>(),
				ComponentType.ReadOnly<ActorForceRotationData>(),
				ComponentType.Exclude<StopRotationData>());
		}

		protected override void OnUpdate()
		{
			var dt = Time.DeltaTime;

			Entities.With(_query).ForEach(
				(Entity entity, ref RotateDirectlyData rotation, ref ActorForceRotationData forceRotation) =>
				{
					rotation.Rotation += (float3)forceRotation.RotationDelta * (forceRotation.RotationSpeed * dt);
				});
		}
	}
}