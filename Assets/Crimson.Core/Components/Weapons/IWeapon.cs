using Assets.Crimson.Core.Common.Types;
using Crimson.Core.Common;
using Crimson.Core.Components;
using System;

namespace Assets.Crimson.Core.Components.Weapons
{
	public interface IWeapon : IActorAbility, IEnableable, IHasComponentName
	{
		event Action OnShot;

		bool IsReady { get; set; }
		public WeaponType Type { get; }

		void AddAmmo(IAmmo ammo);

		void Reload();

		void StartFire();

		void StopFire();
	}
}