using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Crimson.Core.Common
{
	public struct MagnetPointData : IComponentData
	{
		public float3 Direction;
		public float Force;
		public float3 Position;
	}
}