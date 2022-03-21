using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components;
using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.Utils.LowLevel;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Systems
{
	[UpdateInGroup(typeof(FixedUpdateGroup))]
	internal class MagnetRigidbodySystem : ComponentSystem
	{
		protected override void OnUpdate()
		{
			UpdatePointsPositions();
			MagnetObjectsToPoints();
		}

		private bool IsActivated(Entity entity)
		{
			return EntityManager.HasComponent<MagnetWeaponActivated>(entity);
		}

		private void MagnetObjectsToPoints()
		{
			Entities.WithAll<MagnetRigidbodyData>().ForEach(
				(Entity entity, Rigidbody rigidbody, ref MagnetRigidbodyData tag) =>
				{
					if (!IsActivated(tag.Target))
					{
						EntityManager.RemoveComponent<MagnetRigidbodyData>(entity);
						return;
					}

					var point = EntityManager.GetComponentData<MagnetPointData>(tag.Target);
					rigidbody.position = Vector3.Lerp(rigidbody.position, point.Position, Time.DeltaTime * 2f);
				});
		}

		private void UpdatePointsPositions()
		{
			Entities.WithAll<MagnetPointData, MagnetWeaponActivated>().ForEach(
				(Entity entity, ref MagnetPointData data, GravityWeapon weapon) =>
				{
					data.Position = weapon.MagnetPoint;
					EntityManager.SetComponentData(entity, data);
				});
		}
	}
}