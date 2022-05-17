using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Forces
{
	public struct ForceData : IComponentData
	{
		public Vector3 Direction;
		public float Force;
		public int ForceMode;
	}
}