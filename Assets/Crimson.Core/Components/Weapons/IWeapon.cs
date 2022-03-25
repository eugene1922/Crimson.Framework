using Crimson.Core.Components;

namespace Assets.Crimson.Core.Components.Weapons
{
	public interface IWeapon : IActorAbility
	{
		void StopFire();

		void Reload();

		void StartFire();
	}
}