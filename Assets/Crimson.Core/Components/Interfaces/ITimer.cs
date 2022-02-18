using Crimson.Core.Common;

namespace Crimson.Core.Components
{
	public interface ITimer
	{
		void FinishTimer();

		void StartTimer();

		bool TimerActive { get; set; }
		TimerComponent Timer { get; }
	}
}