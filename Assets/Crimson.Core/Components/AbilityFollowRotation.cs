using Crimson.Core.Common;
using Crimson.Core.Enums;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Components
{
	public struct ActorFollowRotationData : IComponentData
	{
		public float3 Origin;
	}

	public struct ActorNoFollowTargetRotationData : IComponentData
	{
	}

	[HideMonoScript]
	public class AbilityFollowRotation : MonoBehaviour, IActorAbility
	{
		[ShowIf("followTarget", TargetType.ComponentName)]
		public string actorWithComponentName;

		[EnumToggleButtons] public TargetType followTarget;
		public bool followX = false;
		public bool followY = true;
		public bool followZ = false;
		public bool hideIfNoTarget = false;
		public bool retainRotationOffset = true;

		[HideIf("followTarget", TargetType.Spawner)]
		[EnumToggleButtons]
		public ChooseTargetStrategy strategy;

		[HideInInspector] public Transform target;

		[ShowIf("followTarget", TargetType.ChooseByTag)]
		[ValueDropdown(nameof(Tags))]
		public string targetTag;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;

			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			dstManager.AddComponentData(entity, new ActorFollowRotationData());
			dstManager.AddComponentData(entity, new ActorNoFollowTargetRotationData());

			dstManager.AddComponentData(entity, new RotateDirectlyData
			{
				Constraints = new bool3(followX, followY, followZ)
			});

			if (hideIfNoTarget)
			{
				gameObject.SetActive(false);
			}
		}

		public void Execute()
		{
		}

		private static IEnumerable Tags()
		{
			return EditorUtils.GetEditorTags();
		}
	}
}