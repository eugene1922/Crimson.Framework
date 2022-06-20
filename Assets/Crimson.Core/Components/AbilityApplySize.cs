using Crimson.Core.Common;
using Sirenix.OdinInspector;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
	[HideMonoScript]
	public class AbilityApplySize : MonoBehaviour, IActorAbilityTarget
	{
		public IActor AbilityOwnerActor { get; set; }
		public IActor Actor { get; set; }
		public IActor TargetActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Execute()
		{
			if (TargetActor == null) return;

			var targetActor = TargetActor.Abilities?.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

			if (targetActor == null)
			{
				return;
			}

			Vector3 scale = Actor.GameObject.transform.localScale;
			float distance = (targetActor.transform.position - Actor.GameObject.transform.position).magnitude;
			scale.z = distance;
			Actor.GameObject.transform.localScale = scale;
		}

	}
}