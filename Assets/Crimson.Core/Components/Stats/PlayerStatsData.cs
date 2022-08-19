using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.Stats
{
	[System.Serializable]
	public struct PlayerStatsData : IComponentData
	{
		[Range(0, 1)] public float CriticalDamageChance;
		public float CriticalDamageMultiplier;
		public float CurrentExperience;
		public MinMaxStat<float> Energy;
		public MinMaxStat<float> Health;
		public int Level;
		public int LevelUpRequiredExperience;
		public int TotalDamageApplied;
	}
}