using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public struct ActivateRagdollData : IComponentData
	{ }

	public struct DeactivateRagdollData : IComponentData
	{ }

	public class AbilityRagdoll : MonoBehaviour, IActorAbility
	{
		[HideInInspector] public Collider[] _colliders;
		[HideInInspector] public Rigidbody[] _rigidbodies;
		public Transform Root;

		public IActor Actor { get; set; }

		[Button]
		public void Activate()
		{
			SetState(true);
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		[Button]
		public void Deactivate()
		{
			SetState(false);
		}

		public void Execute()
		{
		}

		public void SetState(bool state)
		{
			for (var i = 0; i < _colliders.Length; i++)
			{
				_colliders[i].enabled = state;
			}

			for (var i = 0; i < _rigidbodies.Length; i++)
			{
				_rigidbodies[i].isKinematic = !state;
				_rigidbodies[i].useGravity = state;
			}
		}

		[Button]
		private void Setup()
		{
			_colliders = Root.GetComponentsInChildren<Collider>();
			_rigidbodies = Root.GetComponentsInChildren<Rigidbody>();
		}
	}
}