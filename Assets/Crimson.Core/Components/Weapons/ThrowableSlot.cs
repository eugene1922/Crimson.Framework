using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class ThrowableSlot : MonoBehaviour, IActorAbility
	{
		public InputActionReference _executeAction;

		[ValidateInput(nameof(MustBeThrowable), "MonoBehaviours must derive from IThrowable!")]
		public MonoBehaviour ThrowableWeapon;

		private IThrowable _weapon;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			if (_executeAction != null)
			{
				_executeAction.action.performed += ThrowActionHandler;
			}

			if (ThrowableWeapon != null)
			{
				_weapon = (IThrowable)ThrowableWeapon;
			}
		}

		public void Change(IThrowable weapon)
		{
			_weapon = weapon;
		}

		public void Execute()
		{
			_weapon?.Execute();
		}

		private bool MustBeThrowable(MonoBehaviour item)
		{
			return item == null || item is IThrowable;
		}

		private void ThrowActionHandler(InputAction.CallbackContext obj)
		{
			Execute();
		}

		internal void Add(IThrowable item)
		{
			_weapon = item;
		}
	}
}