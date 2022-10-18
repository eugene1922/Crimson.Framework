using Assets.Crimson.Core.Common.Filters;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityFindTargetActor : MonoBehaviour, IActorAbility
	{
		public TagFilter TagFilter;
		public IActor Actor { get; set; }
		public Actor Target { get; internal set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Execute()
		{
		}
	}
}