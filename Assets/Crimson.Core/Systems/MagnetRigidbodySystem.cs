using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components.Tags;
using Assets.Crimson.Core.Components.Weapons;
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
				(Entity entity, Rigidbody rigidbody, ref MagnetRigidbodyData data) =>
				{
					var point = EntityManager.GetComponentData<MagnetPointData>(data.Target);
					if (!IsActivated(data.Target))
					{
						rigidbody.isKinematic = false;
						rigidbody.velocity = point.Force * point.Direction;
						EntityManager.RemoveComponent<MagnetRigidbodyData>(entity);
						return;
					}
					var newPosition = Vector3.Distance(point.Position, rigidbody.position) > 1f ?
						Vector3.Lerp(rigidbody.position, point.Position, Time.DeltaTime * 20f)
						: (Vector3)point.Position;
					rigidbody.isKinematic = true;
					rigidbody.position = newPosition;
				});
		}

		private void UpdatePointsPositions()
		{
			Entities.WithAll<MagnetPointData, MagnetWeaponActivated>().ForEach(
				(Entity entity, ref MagnetPointData data, GravityWeapon weapon) =>
				{
					data.Position = weapon.MagnetPoint;
					data.Direction = weapon.transform.forward;
					EntityManager.SetComponentData(entity, data);
				});
		}
	}
}