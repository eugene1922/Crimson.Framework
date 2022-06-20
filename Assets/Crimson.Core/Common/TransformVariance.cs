using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Crimson.Core.Common
{
	[Serializable]
	public struct TransformVariance
	{
		public Vector3 PostionOffset;
		public Vector3 RotationOffset;
		private Vector3 LastPosition;
		private Vector3 LastRotation;
		public bool Cycled;
		[Range(0f, 1f)]
		public float MemoryKoef;

		internal float3 Generate()
		{
			var vector = GetRandomPosition(PostionOffset);
			var rotation = GetRandomRotation(RotationOffset);
			var eueler = rotation.eulerAngles;
			return math.mul(rotation, vector);
		}

		internal Vector3 GeneratePosition()
		{
			return GetRandomPosition(PostionOffset);
		}

		internal Quaternion GenerateRotation()
		{
			return GetRandomRotation(RotationOffset);
		}

		private Vector3 GetRandomValue(Vector3 sourceOffset, ref Vector3 lastValue)
		{
			Vector3 delta = Cycled ? -lastValue : Vector3.zero;
			Vector3 newValue = new Vector3(
				Random.Range(-sourceOffset.x + delta.x, sourceOffset.x + delta.x),
				Random.Range(-sourceOffset.y + delta.y, sourceOffset.y + delta.y),
				Random.Range(-sourceOffset.z + delta.z, sourceOffset.z + delta.z));
			Vector3 value = Vector3.Lerp(lastValue, newValue, 1 - MemoryKoef);
			lastValue = value;
			return value;
		}

		private Vector3 GetRandomPosition(Vector3 sourceOffset)
		{
			return GetRandomValue(PostionOffset, ref LastPosition);
		}

		private Quaternion GetRandomRotation(Vector3 sourceOffset)
		{
			return Quaternion.Euler(GetRandomValue(sourceOffset, ref LastRotation));
		}
	}
}