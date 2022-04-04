using Assets.Crimson.Core.Common.Magnets;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Systems.Magnets
{
	public class MagnetSnappingSystem : ComponentSystem
	{
		protected override void OnCreate()
		{
		}

		protected override void OnUpdate()
		{
			Entities.WithAll<MagnetSnapData>().ForEach(
				(Entity entity, Rigidbody rigidbody, ref MagnetSnapData snapData) =>
				{
					var targetTransform = EntityManager.GetComponentObject<Transform>(snapData.Target);

					rigidbody.isKinematic = true;
					rigidbody.useGravity = false;
					var distance = Vector3.Distance(targetTransform.position, rigidbody.position);
					if (distance > snapData.Threshold)
					{
						rigidbody.position = Vector3.Lerp(rigidbody.position, targetTransform.position, Time.DeltaTime * 2);
					}
					else
					{
						rigidbody.isKinematic = false;
						rigidbody.useGravity = true;
						EntityManager.RemoveComponent<MagnetSnapData>(entity);
						rigidbody.position = targetTransform.position;
					}
				});
		}
	}
}