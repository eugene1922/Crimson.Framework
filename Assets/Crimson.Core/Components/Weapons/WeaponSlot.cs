using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
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

		[HideInInspector] public UIReceiverList UIReceiverList = new UIReceiverList();

		[CastToUI("CurrentWeapon")]
		public IWeapon _weapon;

		public IActor Actor { get; set; }

		public bool IsEnable { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			UIReceiverList.Init(this, entity);
			if (_executeAction != null)
			{
				_executeAction.action.performed += ExectionActionPerformed;
				_executeAction.action.canceled += ExectionActionCanceled;
			}
			if (_reloadAction != null)
			{
				_reloadAction.action.performed += ReloadActionHandler;
			}

			if (Weapon != null)
			{
				Change((IWeapon)Weapon);
			}
		}

		public void Change(IWeapon weapon)
		{
			if (!IsEnable)
			{
				return;
			}
			if (_weapon != null)
			{
				_weapon.IsEnable = false;
			}
			_weapon = weapon;
			weapon.IsEnable = true;
			UIReceiverList.UpdateUIData("CurrentWeapon");
		}

		public void Execute()
		{
			if (!IsEnable)
			{
				return;
			}
			_weapon?.StartFire();
		}

		private void ExectionActionCanceled(InputAction.CallbackContext obj)
		{
			if (!IsEnable)
			{
				return;
			}
			_weapon?.StopFire();
		}

		private void ExectionActionPerformed(InputAction.CallbackContext obj)
		{
			if (!IsEnable)
			{
				return;
			}
			Execute();
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
	}
}