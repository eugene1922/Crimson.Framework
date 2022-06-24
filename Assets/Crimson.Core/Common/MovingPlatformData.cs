using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Crimson.Core.Common
{
	public struct MovingPlatformData : IComponentData
	{
		public float3 Position;
		public quaternion Rotation;

		public MovingPlatformData(Transform transform)
		{
			Position = transform.position;
			Rotation = transform.rotation;
		}
	}
}