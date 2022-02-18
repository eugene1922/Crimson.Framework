using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Crimson.Core.Editor
{
	public class RagdollTools : MonoBehaviour
	{
		[Button]
		public void DisableColliders()
		{
		}

		[Button]
		[ContextMenu(nameof(Remove))]
		public void Remove()
		{
			var rigidbodies = GetComponentsInChildren<Rigidbody>();
			var colliders = GetComponentsInChildren<Collider>();
			var joints = GetComponentsInChildren<CharacterJoint>();

			DestroyObjects(joints);
			DestroyObjects(colliders);
			DestroyObjects(rigidbodies);
		}

		[Button]
		public void RemoveRigidbody()
		{
			var rigidbodies = GetComponentsInChildren<Rigidbody>();
			DestroyObjects(rigidbodies);
		}

		private void DestroyObjects(params Object[] targets)
		{
			for (var i = 0; i < targets.Length; i++)
			{
				DestroyImmediate(targets[i]);
			}
		}
	}
}