using Crimson.Core.Common;
using Crimson.Core.Components;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Components
{
	public class AbilityButtonEventInputRef : MonoBehaviour, IActorAbility
	{
		public InputActionReference _input;

		public UnityEvent OnPress;
		public UnityEvent OnRelease;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;

			_input.action.performed += StartAction;
			_input.action.canceled += StopAction;
		}

		public void Execute()
		{
		}

		private bool MustBeAbility(List<MonoBehaviour> a)
		{
			return !a.Exists(t => !(t is IActorAbility)) || a.Count == 0;
		}

		private void StartAction(InputAction.CallbackContext context)
		{
			OnPress?.Invoke();
		}

		private void StopAction(InputAction.CallbackContext context)
		{
			OnRelease?.Invoke();
		}
	}
}