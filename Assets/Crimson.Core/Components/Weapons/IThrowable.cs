using Crimson.Core.Common;
using Crimson.Core.Components;

namespace Assets.Crimson.Core.Components.Weapons
{
	public interface IThrowable : IActorAbility, IHasComponentName
	{
		void AddAmmo(IAmmo ammo);

		void Throw();
	}
}