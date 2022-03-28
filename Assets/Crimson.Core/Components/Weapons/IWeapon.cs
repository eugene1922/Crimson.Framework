using Crimson.Core.Components;

namespace Assets.Crimson.Core.Components.Weapons
{
	public interface IWeapon : IActorAbility
	{
		bool IsEnable { get; set; }

		void Reload();

		void StartFire();

		void StopFire();
	}
}