using System.Collections.Generic;
using UnityEngine;

namespace Crimson.Core.Common
{
	public interface IPerkAbilityBindable : IBindable
	{
		List<MonoBehaviour> PerkRelatedComponents { get; set; }
	}
}