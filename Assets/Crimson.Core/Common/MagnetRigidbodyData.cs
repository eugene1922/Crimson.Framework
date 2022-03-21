using Unity.Entities;

namespace Assets.Crimson.Core.Common
{
	public struct MagnetRigidbodyData : IComponentData
	{
		public Entity Target;

		public MagnetRigidbodyData(Entity entity)
		{
			Target = entity;
		}
	}
}