using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace Assets.Crimson.Core.Components
{
	[HideMonoScript]
	public class AbilityChainActions : TimerBaseBehaviour, IActorAbility
	{
		public bool ExecuteOnStart = false;
		public AbilityChainSettings Settings = new AbilityChainSettings();
		private List<IActorAbility> _abilities;
		private int _currentAbility;
		private int _loopsCount;
		private bool _isInfinateLoop;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_abilities = Settings.Actions.Cast<IActorAbility>().Where(s => s != null).ToList();
			ResetTimer();
			StartTimer();
			if (ExecuteOnStart)
			{
				Execute();
			}
		}

		public void Execute()
		{
			if (Settings.Actions.Count == 0)
			{
				return;
			}

			_loopsCount = Settings.LoopCount;
			_isInfinateLoop = _loopsCount == 0;

			Timer.TimedActions.AddAction(ExecuteAbility, Settings.StartupDelay);
		}

		private void ExecuteAbility()
		{
			_abilities[_currentAbility].Execute();

			if (_currentAbility < _abilities.Count - 1)
			{
				_currentAbility++;
			}
			else
			{
				if (_isInfinateLoop)
				{
					_currentAbility = 0;
				}

				if (_loopsCount > 0)
				{
					_loopsCount--;
					_currentAbility = 0;
				}

				if (_currentAbility != 0)
				{
					return;
				}
			}

			Timer.TimedActions.AddAction(ExecuteAbility, Settings.Delay);
		}
	}
}