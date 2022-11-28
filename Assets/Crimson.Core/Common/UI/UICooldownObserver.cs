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
		public UnityEvent OnStart;
		public UnityEvent OnFinish;
		public UnityEvent<float> OnRefresh;
		private float _lastState = 1.0f;

		private void Awake()
		{
			OnFinish?.Invoke();
		}

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
			var result = state.CompareTo(_lastState);
			if (result != 0)
			{
				_lastState = state;
				if (_lastState == 1)
				{
					OnFinish?.Invoke();
				}
				if (result < 0)
				{
					OnStart?.Invoke();
				}
			}

			OnRefresh?.Invoke(_lastState);
		}
	}
}