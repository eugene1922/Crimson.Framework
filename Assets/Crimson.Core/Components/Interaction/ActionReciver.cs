using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
	[Serializable]
	public struct ActionReciver
	{
		[ValidateInput(nameof(MustBeAbilityTarget), "Ability MonoBehaviours must derive from IActorAbilityTarget!")]
		public List<MonoBehaviour> Actions;

		private bool MustBeAbilityTarget(List<MonoBehaviour> a)
		{
			return !a.Exists(t => !(t is IActorAbilityTarget)) || a.Count == 0;
		}
	}
}