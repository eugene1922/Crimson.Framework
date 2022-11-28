using Crimson.Core.Common;

namespace Assets.Crimson.Core.Common.Interfaces
{
	public interface ICooldownObservable : IHasComponentName
	{
		public float State { get; }
	}
}