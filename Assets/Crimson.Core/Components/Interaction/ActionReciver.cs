using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
	[Serializable]
	public struct ActionReciver
	{
		[ValidateInput(nameof(MustBeAbilityTarget), "Ability MonoBehaviours must derive from IActorAbilityTarget or IActorAbility!")]
		public MonoBehaviour[] Actions;

		private bool MustBeAbilityTarget(MonoBehaviour[] a)
		{
			return a == null || a.All(s => s is IActorAbility);
		}
	}
}