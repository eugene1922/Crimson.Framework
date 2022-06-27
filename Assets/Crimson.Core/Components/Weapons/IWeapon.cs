using Assets.Crimson.Core.Common.Types;
using Crimson.Core.Components;
using System;

namespace Assets.Crimson.Core.Components.Weapons
{
	public interface IWeapon : IActorAbility, IEnableable
	{
		public WeaponType Type { get; }

		void Reload();

		void StartFire();

		void StopFire();

		event Action OnShot;
	}
}