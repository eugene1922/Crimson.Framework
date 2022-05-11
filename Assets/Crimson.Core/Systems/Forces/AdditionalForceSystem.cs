using Assets.Crimson.Core.Components.Forces;
using Crimson.Core.Utils.LowLevel;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Systems.Forces
{
	[UpdateInGroup(typeof(FixedUpdateGroup))]
	internal class AdditionalForceSystem : ComponentSystem
	{
		private EntityQuery _forceQuery;
		private EntityQuery _sourcesQuery;

		protected override void OnCreate()
		{
			_sourcesQuery = GetEntityQuery(
				ComponentType.ReadOnly<InwardForceSourceTag>());

			_forceQuery = GetEntityQuery(
				ComponentType.ReadOnly<ForceData>(),
				ComponentType.ReadOnly<Rigidbody>());
		}

		protected override void OnUpdate()
		{
			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			Entities.With(_sourcesQuery).ForEach(
				(AbilityInwardForce ability) =>
				{
					ability.AddForceInRadius();
				});

			Entities.With(_forceQuery).ForEach(
				(Entity entity, Rigidbody rigidBody, ref ForceData data) =>
				{
					if (rigidBody == null)
					{
						return;
					}
					rigidBody.AddForce(data.Direction * data.Force, (ForceMode)data.ForceMode);
					dstManager.RemoveComponent<ForceData>(entity);
				}
			);
		}
	}
}