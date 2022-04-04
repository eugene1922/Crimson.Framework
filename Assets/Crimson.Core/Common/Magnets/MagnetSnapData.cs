using Unity.Entities;

namespace Assets.Crimson.Core.Common.Magnets
{
	public struct MagnetSnapData : IComponentData
	{
		public Entity Target;
		public float Threshold;

		public MagnetSnapData(Entity entity, float threshold)
		{
			Target = entity;
			Threshold = threshold;
		}
	}
}