using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
	[HideMonoScript]
	public class InteractionZone : MonoBehaviour, IActorAbility
	{
		public EntityManager _dstManager;
		public Vector3 Offset;
		public Vector3 Size = Vector3.one;
		public IActor Actor { get; set; }
		public Vector3 Position => transform.position + Offset;

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		public void Execute()
		{
			_dstManager.AddComponent<ActivatedInteractionZoneTag>(Actor.ActorEntity);
		}

#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Offset, Size);
		}

#endif
	}
}