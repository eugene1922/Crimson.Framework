using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class WeaponSlot : MonoBehaviour, IActorAbility
	{
		public InputActionReference _executeAction;
		public InputActionReference _reloadAction;

		[ValidateInput(nameof(MustBeWeapon), "Perk MonoBehaviours must derive from IWeapon!")]
		public MonoBehaviour Weapon;

		public IWeapon _weapon { get; private set; }
		public IActor Actor { get; set; }

		public bool IsEnable { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			if (_executeAction != null)
			{
				_executeAction.action.performed += ShootActionHandler;
			}
			if (_reloadAction != null)
			{
				_reloadAction.action.performed += ReloadActionHandler;
			}

			if (Weapon != null)
			{
				_weapon = (IWeapon)Weapon;
			}
		}

		public void Change(IWeapon weapon)
		{
			if (!IsEnable)
			{
				return;
			}
			_weapon = weapon;
		}

		public void Execute()
		{
			if (!IsEnable)
			{
				return;
			}
			_weapon?.Execute();
		}

		private bool MustBeWeapon(MonoBehaviour item)
		{
			return item == null || item is IWeapon;
		}

		private void ReloadActionHandler(InputAction.CallbackContext obj)
		{
			if (!IsEnable)
			{
				return;
			}
			_weapon?.Reload();
		}

		private void ShootActionHandler(InputAction.CallbackContext obj)
		{
			if (!IsEnable)
			{
				return;
			}
			Execute();
		}
	}
}