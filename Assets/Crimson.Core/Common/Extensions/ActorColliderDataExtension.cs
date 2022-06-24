using Crimson.Core.Components;
using Crimson.Core.Enums;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Crimson.Core.Common.Extensions
{
	public static class ActorColliderDataExtension
	{
		public static int GetResults(this ActorColliderData colliderData, Transform parent, Collider[] results)
		{
			float3 position = parent.position;
			var rotation = parent.rotation;
			int size;
			switch (colliderData.ColliderType)
			{
				case ColliderType.Sphere:
					size = Physics.OverlapSphereNonAlloc(colliderData.SphereCenter + position,
						colliderData.SphereRadius, results);
					break;

				case ColliderType.Capsule:
					var center =
						(colliderData.CapsuleStart + position + (colliderData.CapsuleEnd + position)) / 2f;
					var point1 = colliderData.CapsuleStart + position;
					var point2 = colliderData.CapsuleEnd + position;
					point1 = (float3)(rotation * (point1 - center)) + center;
					point2 = (float3)(rotation * (point2 - center)) + center;
					size = Physics.OverlapCapsuleNonAlloc(point1,
						point2,
						colliderData.CapsuleRadius, results);
					break;

				case ColliderType.Box:
					size = Physics.OverlapBoxNonAlloc(colliderData.BoxCenter + position,
						colliderData.BoxHalfExtents, results, colliderData.BoxOrientation * rotation);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			return size;
		}
	}
}