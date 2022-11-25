using Assets.Crimson.Core.Common.Weapons;
using Assets.Crimson.Core.Components.Tags.Weapons;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class WeaponSlot : TimerBaseBehaviour, IActorAbility
	{
		public InputActionReference _executeAction;
		public InputActionReference _reloadAction;

		[CastToUI("CurrentWeapon")]
		public IWeapon _weapon;

		public float _weaponChangeDuration = 1f;

		public bool EnableOnStart;

		[HideInInspector] public UIReceiverList UIReceiverList = new UIReceiverList();

		[ValidateInput(nameof(MustBeWeapon), "Perk MonoBehaviours must derive from IWeapon!")]
		public MonoBehaviour Weapon;

		private Entity _entity;
		private EntityManager _entityManager;

		public IActor Actor { get; set; }

		public bool IsEnable { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
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

			_entityManager.AddComponentData(_entity, new EquipedWeaponData());

			Timer.Start();

			IsEnable = EnableOnStart;

			if (Weapon != null)
			{
				Change((IWeapon)Weapon, EnableOnStart);
			}
		}

		public void Change(IWeapon weapon, bool skipAnimation = false)
		{
			if (!IsEnable)
			{
				return;
			}
			_weapon?.StopFire();
			var weaponData = new EquipedWeaponData();
			if (_weapon != null)
			{
				_weapon.IsEnable = false;
				weaponData.Previous = (byte)_weapon.Type;
			}
			_weapon = weapon;
			_weapon.IsEnable = true;
			weaponData.Current = (byte)weapon.Type;
			weaponData.NeedAnimation = !skipAnimation;
			_entityManager.AddComponentData(_entity, weaponData);
			_entityManager.AddComponentData(_entity, new StartChangeWeaponAnimTag());
			if (!skipAnimation)
			{
				Timer.TimedActions.AddAction(EndChangeWeapon, _weaponChangeDuration);
			}

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

		private void EndChangeWeapon()
		{
			_entityManager.AddComponentData(_entity, new EndChangeWeaponAnimTag());
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