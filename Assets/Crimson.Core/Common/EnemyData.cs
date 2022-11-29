using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Tags
{
	public struct EnemyData : IComponentData
	{
		public float3 LockRange;
		public float3 Offset;

		public EnemyData(Vector3 offset, float3 lockRange)
		{
			Offset = offset;
			LockRange = lockRange;
		}
	}
}