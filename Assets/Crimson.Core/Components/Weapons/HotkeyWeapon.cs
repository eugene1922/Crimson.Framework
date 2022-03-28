using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class HotkeyWeapon : MonoBehaviour, IActorAbility
	{
		public InputActionReference _activateGravigunAction;
		public InputActionReference _changeAction;
		public WeaponSlot _slot;

		[ValidateInput(nameof(MustBeWeapon), "Perk MonoBehaviours must derive from IWeapon!")]
		public List<MonoBehaviour> Weapons;

		private readonly List<IWeapon> _weapons = new List<IWeapon>();

		private IWeapon _gravityGun;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_weapons.AddRange(Weapons.Cast<IWeapon>());
			if (_changeAction != null)
			{
				_changeAction.action.performed += ChangeActionHandler;
			}
			if (_activateGravigunAction != null)
			{
				_activateGravigunAction.action.performed += ToggleGravigunHandler;
			}
			_slot.IsEnable = true;
		}

		public void Execute()
		{
		}

		internal void Add(IWeapon copy)
		{
			_weapons.Add(copy);
			if (copy is GravityWeapon)
			{
				_gravityGun = copy;
			}
			SelectNextWeapon();
		}

		private void ChangeActionHandler(InputAction.CallbackContext obj)
		{
			SelectNextWeapon();
		}

		private bool MustBeWeapon(List<MonoBehaviour> actions)
		{
			foreach (var action in actions)
			{
				if (action is IWeapon || action is null)
				{
					continue;
				}

				return false;
			}

			return true;
		}

		private void SelectNextWeapon()
		{
			var currentIndex = _weapons.IndexOf(_slot._weapon);
			if (currentIndex == -1)
			{
				_slot.Change(_weapons[0]);
			}
			else
			{
				var newWeaponIndex = currentIndex + 1;
				_slot.Change(_weapons[newWeaponIndex % _weapons.Count]);
			}
		}

		private void ToggleGravigunHandler(InputAction.CallbackContext obj)
		{
			if (_gravityGun == null)
			{
				return;
			}

			_slot.Change(_gravityGun);
		}
	}
}