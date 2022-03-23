using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class GravityWeapon : MonoBehaviour, IActorAbility
	{
		public InputActionReference _activationAction;
		public Entity _entity;
		public float _maxDistance = 10;
		public Vector3 MagnetOffset = Vector3.forward;

		private EntityManager _entityManager;
		[SerializeField] private float _force = 10;
		private bool _isEnable;
		private RaycastHit[] _raycastResults = new RaycastHit[25];
		public IActor Actor { get; set; }

		public bool IsEnable
		{
			get => _isEnable;
			set
			{
				_isEnable = value;
				if (!value)
				{
					Deactivate();
				}
			}
		}

		public Vector3 MagnetPoint => transform.TransformPoint(MagnetOffset);

		public void Activate()
		{
			_entityManager.AddComponentData(_entity, new MagnetWeaponActivated());
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;

			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			var data = new MagnetPointData()
			{
				Force = _force,
			};
			_entityManager.AddComponentData(_entity, data);

			_activationAction.action.performed += ActivateActionHandler;
		}

		public void Deactivate()
		{
			if (_entity != Entity.Null && _entityManager != null)
			{
				_entityManager.RemoveComponent<MagnetWeaponActivated>(_entity);
			}
		}

		public void Execute()
		{
			if (!IsEnable)
			{
				return;
			}
			if (_entityManager.HasComponent<MagnetWeaponActivated>(_entity))
			{
				Deactivate();
			}
			else
			{
				Activate();
				TryGrabObject();
			}
		}

		private void ActivateActionHandler(InputAction.CallbackContext obj)
		{
			if (!IsEnable)
			{
				return;
			}
			Execute();
		}

#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireSphere(MagnetOffset, 1);
		}

#endif

		private void TryGrabObject()
		{
			var ray = new Ray(transform.position, transform.forward);
			var resultsCount = Physics.RaycastNonAlloc(ray, _raycastResults, _maxDistance);
			if (resultsCount == 0)
			{
				return;
			}
			var result = _raycastResults[resultsCount - 1];
			var target = result.rigidbody;
			if (target == null)
			{
				return;
			}
			var abilityMagnet = target.GetComponent<AbilityMagnet>();
			if (abilityMagnet != null)
			{
				abilityMagnet.MagnetTo(_entity);
			}
		}
	}
}