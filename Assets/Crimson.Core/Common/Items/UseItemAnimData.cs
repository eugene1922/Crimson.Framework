using Unity.Entities;

namespace Assets.Crimson.Core.Common.Items
{
	public struct UseItemAnimData : IComponentData
	{
		public byte Type;

		public UseItemAnimData(byte type)
		{
			Type = type;
		}
	}
}