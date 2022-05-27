using Unity.Entities;

namespace Assets.Crimson.Core.Common.ComponentDatas
{
	public struct OverdamageData : IComponentData
	{
		public float Damage;

		public OverdamageData(float value)
		{
			Damage = value;
		}
	}
}