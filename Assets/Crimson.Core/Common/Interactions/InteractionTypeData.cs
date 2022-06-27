using Unity.Entities;

namespace Assets.Crimson.Core.Common.Interactions
{
	public struct InteractionTypeData : IComponentData
	{
		public byte Type;

		public InteractionTypeData(byte type)
		{
			Type = type;
		}
	}
}