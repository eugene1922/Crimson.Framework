using Crimson.Core.Components;

namespace Assets.Crimson.Core.Components.Weapons
{
	public interface IWeapon : IActorAbility, IEnableable
	{

		void Reload();

		void StartFire();

		void StopFire();
	}
}