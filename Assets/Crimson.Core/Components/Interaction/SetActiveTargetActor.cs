using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
	public class SetActiveTargetActor : MonoBehaviour, IActorAbilityTarget
	{
		public bool State;

		public IActor Actor { get; set; }
		public IActor TargetActor { get; set; }
		public IActor AbilityOwnerActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Execute()
		{
			if (TargetActor == null)
			{
				return;
			}

			TargetActor.GameObject.SetActive(State);
		}
	}
}