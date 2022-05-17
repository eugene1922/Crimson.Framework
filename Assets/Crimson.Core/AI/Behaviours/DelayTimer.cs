using UnityEngine;

namespace Assets.Crimson.Core.AI.Behaviours
{
	public class DelayTimer
	{
		private float _delayTime;
		private float _startTime;

		public bool IsExpire => Time.realtimeSinceStartup - _startTime > _delayTime;

		public bool IsStopped => _startTime == default;

		public void Stop()
		{
			_startTime = default;
		}

		public void Start(float delay)
		{
			_delayTime = delay;
			_startTime = Time.realtimeSinceStartup;
		}
	}
}