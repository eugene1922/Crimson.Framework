using Unity.Entities;

namespace Assets.Crimson.Core.Components.Stats
{
	[System.Serializable]
	public struct PlayerStatsData : IComponentData
	{
		public int CurrentExperience;
		public MinMaxStat<int> Energy;
		public MinMaxStat<int> Health;
		public int Level;
		public int LevelUpRequiredExperience;
		public int TotalDamageApplied;
	}
}