using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class HotkeyWeapon : MonoBehaviour, IActorAbility
	{
		public InputActionReference _activateAction;
		public WeaponSlot _slot;

		[ValidateInput(nameof(MustBeWeapon), "Perk MonoBehaviours must derive from IWeapon!")]
		public MonoBehaviour Weapon;

		private IWeapon _weapon;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			if (Weapon != null)
			{
				_weapon = (IWeapon)Weapon;
			}
			if (_activateAction != null)
			{
				_activateAction.action.performed += ActivateActionHandler;
			}
		}

		public void Execute()
		{
			if (_weapon != null)
			{
				_slot.Change(_weapon);
			}
		}

		private void ActivateActionHandler(InputAction.CallbackContext obj)
		{
			Execute();
		}

		private bool MustBeWeapon(MonoBehaviour item)
		{
			return item == null || item is IWeapon;
		}
	}
}