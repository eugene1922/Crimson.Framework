using Assets.Crimson.Core.Components.Interfaces;
using Crimson.Core.Components;
using System.Collections.Generic;

namespace Crimson.Core.Utils
{
	public static class AIUtils
	{
		public static bool ActionPossible(this List<IActorAbility> abilities)
		{
			var possible = false;

			for (var i = 0; i < abilities.Count; i++)
			{
				var ability = abilities[i];
				var enableable = ability as IEnableable;
				var blockable = ability as IBlockable;

				if (blockable != null && blockable.IsBlocked)
				{
					possible |= !blockable.IsBlocked;
				}
				else if (enableable != null)
				{
					possible |= enableable.IsEnable;
				}

				if (blockable == null && enableable == null)
				{
					possible = true;
				}

				return possible;
			}

			return false;
		}
	}
}