using UnityEngine;

namespace Crimson.Core.Common
{
	public interface IPerkUpgrade
	{
		string PerkName { get; }
		Sprite PerkImage { get; }
		GameObject PerkPrefab { get; }

		void SpawnPerk(IActor target);
	}
}