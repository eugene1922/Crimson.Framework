using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	[HideMonoScript]
	public class AbilityLookRotation : MonoBehaviour, IActorAbility
	{
		public float Speed = 5;
		public Vector3 Offset;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;

			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			dstManager.AddComponentData(entity, new FollowLookRotationData()
			{
				Offset = Offset,
				Speed = Speed
			});
		}

		public void Execute()
		{
		}
	}
}