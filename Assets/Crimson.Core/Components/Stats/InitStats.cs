namespace Assets.Crimson.Core.Components.Stats
{
	public struct InitStats
	{
		public int CurrentExperience;
		public MinMaxStat<int> Energy;
		public MinMaxStat<int> Health;
		public int Level;
		public int LevelUpRequiredExperience;
		public int TotalDamageApplied;

		public PlayerStatsData CreateComponentData()
		{
			return new PlayerStatsData()
			{
				CurrentExperience = CurrentExperience,
				Energy = Energy,
				Health = Health,
				Level = Level,
				LevelUpRequiredExperience = LevelUpRequiredExperience,
				TotalDamageApplied = TotalDamageApplied
			};
		}
	}
}