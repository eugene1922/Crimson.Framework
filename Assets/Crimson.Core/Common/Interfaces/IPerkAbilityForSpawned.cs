using UnityEngine;

namespace Crimson.Core.Common
{
	public interface IPerkAbilityForSpawned : IPerkAbility
	{
		void AddCollisionAction(GameObject target);
	}
}