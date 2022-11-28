using Assets.Crimson.Core.Components.Interfaces;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using System;

namespace Crimson.Core.Common
{
	public class CooldownBehaviour : TimerBaseBehaviour, IBlockable, IEnableable
	{
		private float _cooldownTime;
		public bool IsBlocked { get; set; }
		public bool IsEnable { get; set; } = true;

		public float State
		{
			get
			{
				var state = 1.0f;

				var action = Timer.TimedActions.Find(s => s.Act == FinishTimer);
				if (action.Delay > 0)
				{
					state = (_cooldownTime - action.Delay) / _cooldownTime;
				}

				return state;
			}
		}

		public void ApplyActionWithCooldown(float cooldownTime, Action action)
		{
			if (!IsEnable || IsBlocked) return;

			action.Invoke();

			if (Math.Abs(cooldownTime) < 0.1f) return;

			_cooldownTime = cooldownTime;
			StartTimer();
			Timer.TimedActions.AddAction(FinishTimer, cooldownTime);
		}

		public override void FinishTimer()
		{
			base.FinishTimer();
			IsEnable = true;
		}

		public override void StartTimer()
		{
			base.StartTimer();
			IsEnable = false;
		}
	}
}