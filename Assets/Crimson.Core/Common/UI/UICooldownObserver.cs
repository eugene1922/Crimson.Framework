using Assets.Crimson.Core.Common.Interfaces;
using Crimson.Core.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Crimson.Core.Common.UI
{
	[HideMonoScript]
	public class UICooldownObserver : MonoBehaviour, IUIObserver
	{
		public string AbilityComponentName;
		public UnityEvent<float> OnRefresh;

		public void Refresh(IActor actor)
		{
			var abilities = actor.Abilities;
			ICooldownObservable target = null;
			for (var i = 0; i < abilities.Count; i++)
			{
				var ability = abilities[i];
				if (ability is ICooldownObservable observable)
				{
					if (observable.ComponentName == AbilityComponentName)
					{
						target = observable;
						break;
					}
				}
			}

			if (target == null)
			{
				return;
			}

			var state = target.State;

			OnRefresh?.Invoke(state);
		}
	}
}