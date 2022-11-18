using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Common.Types;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class MeleeWeapon :
		MonoBehaviour,
		IWeapon
	{

		public WeaponType _weaponType;

		public InputActionReference _activationAction;
		public Entity _entity;


		private EntityManager _entityManager;

		[ValidateInput(nameof(MustBeAbility), "Ability MonoBehaviours must derive from IActorAbility!")]
		public MonoBehaviour abilityOnShot;


		private bool _isEnable;
		private RaycastHit[] _raycastResults = new RaycastHit[25];

		public event System.Action OnShot;

		public IActor Actor { get; set; }

		public bool IsEnable { get; set; }

		public WeaponType Type => _weaponType;

		public void Activate()
		{
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;

			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			if (_activationAction != null && _activationAction.action != null)
				_activationAction.action.performed += ActivateActionHandler;
		}

		public void Deactivate()
		{
		}

		public void Execute()
		{
		}

		public void Reload()
		{
		}

		public void StartFire()
		{
			Activate();
		}

		public void StopFire()
		{
			if (abilityOnShot != null) ((IActorAbility)abilityOnShot).Execute();

			Deactivate();
		}

		private void ActivateActionHandler(InputAction.CallbackContext obj)
		{
			if (!IsEnable)
			{
				return;
			}
			Execute();
		}


		private bool MustBeAbility(MonoBehaviour a)
		{
			return (a is IActorAbility) || (a is null);
		}
	}
}