using Crimson.Core.Common;

namespace Assets.Crimson.Core.Common.Interfaces
{
	public interface IUIObserver
	{
		void Refresh(IActor owner);
	}
}