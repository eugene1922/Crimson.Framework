using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.Stats
{
	[System.Serializable]
	public struct PlayerStatsData : IComponentData
	{
		[Range(0, 1)] public float CriticalDamageChance;
		public float CriticalDamageMultiplier;
		public int CurrentExperience;
		public MinMaxStat<int> Energy;
		public MinMaxStat<int> Health;
		public int Level;
		public int LevelUpRequiredExperience;
		public int TotalDamageApplied;
	}
}