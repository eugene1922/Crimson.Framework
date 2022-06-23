using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Crimson.Core.Components.Tags
{
	public struct FollowLookRotationData : IComponentData
	{
		public float3 Offset;
		public float Speed;
	}
}