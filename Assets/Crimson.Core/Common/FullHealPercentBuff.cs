using Unity.Entities;

namespace Assets.Crimson.Core.Common
{
	public struct FullHealPercentBuff : IComponentData
	{
		public float PercentValue;
		public float Timeout;

		public FullHealPercentBuff(float percent) : this()
		{
			PercentValue = percent;
		}
	}
}