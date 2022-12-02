using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Components
{
	public class AbilityButtonInputRef : MonoBehaviour, IActorAbility
	{
		public InputActionReference _input;

		[ValidateInput(nameof(MustBeAbility), "Ability MonoBehaviours must derive from IActorAbility!")]
		public MonoBehaviour[] actions;

		private ActorAbilityList _actions;
		private EntityManager _entityManager;
		public IActor Actor { get; set; }
		public Entity Entity { get; private set; }
		public bool IsActivated { get; private set; }

		private bool IsInvalidActor => _entityManager == null
			|| _entityManager.HasComponent<DeadActorTag>(Actor.ActorEntity)
			|| _entityManager.HasComponent<StopInputTag>(Actor.ActorEntity);

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Entity = entity;
			Actor = actor;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			if (_input != null)
			{
				_input.action.performed += StartAction;
				_input.action.canceled += StopAction;
				_input.action.Enable();
			}

			_actions = new ActorAbilityList(actions);
		}

		public void Execute()
		{
			_actions.Execute();
		}

		private bool MustBeAbility(MonoBehaviour[] items)
		{
			return items == null || items.All(s => s is IActorAbility);
		}

		private void OnDestroy()
		{
			if (_input == null)
			{
				return;
			}
			_input.action.performed -= StartAction;
			_input.action.canceled -= StopAction;
		}

		private void StartAction(InputAction.CallbackContext context)
		{
			if (IsInvalidActor)
			{
				return;
			}
			IsActivated = true;
			_entityManager.AddComponentData(Entity, new InputExecuteTag());
		}

		private void StopAction(InputAction.CallbackContext context)
		{
			if (IsInvalidActor)
			{
				return;
			}
			_entityManager.RemoveComponent<InputExecuteTag>(Entity);
			IsActivated = false;
		}
	}
}