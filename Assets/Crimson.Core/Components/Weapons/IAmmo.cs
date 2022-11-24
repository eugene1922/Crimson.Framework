using Crimson.Core.Common;

namespace Assets.Crimson.Core.Components.Weapons
{
	public interface IAmmo : IHasComponentName
	{
		public int Value { get; }
	}
}