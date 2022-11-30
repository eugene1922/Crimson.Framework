using Crimson.Core.Common;

namespace Assets.Crimson.Core.Components.Weapons
{
	public interface IAmmo
	{
		public IHasComponentName Target { get; }
		public int Value { get; }
	}
}