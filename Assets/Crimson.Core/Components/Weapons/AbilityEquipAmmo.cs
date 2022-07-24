using Assets.Crimson.Core.Common.Interfaces;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class AbilityEquipAmmo : MonoBehaviour, IActorAbility, IEquipable
	{
		[ValidateInput(nameof(MustBeWeapon), "Perk MonoBehaviours must derive from IWeapon or IThrowable!"), OnValueChanged(nameof(ChangeTarget))]
		public MonoBehaviour TargetWeapon;

		[ReadOnly, ShowInInspector] private string _componentName;

		public string TargetName => TargetWeapon != null ? (TargetWeapon as IHasComponentName).ComponentName : null;

		public int Count;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Equip(IActor target)
		{
			if (TargetWeapon == null)
			{
				return;
			}

			var hotkeyWeapon = target.GameObject.GetComponent<HotkeyWeapon>();
			if (hotkeyWeapon != null)
			{
				if (TargetWeapon is IWeapon)
				{
					AddToWeapon(hotkeyWeapon);
				}

				if (TargetWeapon is IThrowable)
				{
					AddToThrowable(hotkeyWeapon);
				}

				hotkeyWeapon.UpdateUI();
			}
		}

		private void AddToThrowable(HotkeyWeapon hotkeyWeapon)
		{
			var name = TargetName;
			var throwables = hotkeyWeapon.EquipedThrowables;
			for (var i = 0; i < throwables.Count; i++)
			{
				var weapon = throwables[i];
				var weaponName = weapon as IHasComponentName;
				if (weaponName.ComponentName.Contains(name))
				{
					var clip = weapon as IHasClip;
					clip.ClipData.Add(Count);
					break;
				}
			}
		}

		private void AddToWeapon(HotkeyWeapon hotkeyWeapon)
		{
			var name = TargetName;
			var weapons = hotkeyWeapon.EquipedWeapons;
			for (var i = 0; i < weapons.Count; i++)
			{
				var weapon = weapons[i];
				var weaponName = weapon as IHasComponentName;
				if (weaponName.ComponentName.Contains(name))
				{
					var clip = weapon as IHasClip;
					clip.ClipData.Add(Count);
					break;
				}
			}
		}

		public void Execute()
		{
		}

		private bool MustBeWeapon(MonoBehaviour item)
		{
			return item == null || item is IWeapon || item is IThrowable;
		}

		private void ChangeTarget()
		{
			if (!MustBeWeapon(TargetWeapon))
			{
				TargetWeapon = TargetWeapon.GetComponentsInChildren<MonoBehaviour>().FirstOrDefault(s => s is IWeapon || s is IThrowable);
			}

			_componentName = TargetWeapon != null ? (TargetWeapon as IHasComponentName).ComponentName : string.Empty;
		}

#if UNITY_EDITOR

		private void OnValidate()
		{
			if (TargetWeapon == null)
			{
				var weapon = GetComponent<IWeapon>();
				TargetWeapon = weapon as MonoBehaviour;
			}
			else
			{
				_componentName = (TargetWeapon as IHasComponentName).ComponentName;
			}
		}

#endif
	}
}