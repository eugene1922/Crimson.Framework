using Unity.Entities;

namespace Crimson.Core.Components.Stats
{
	[System.Serializable]
	public struct PlayerStatsData : IComponentData
	{
		public int CriticalDamageMultiplier;
		public int CurrentExperience;
		public MinMaxStat<int> Energy;
		public MinMaxStat<int> Health;
		public int Level;
		public int LevelUpRequiredExperience;
		public int TotalDamageApplied;
	}
}