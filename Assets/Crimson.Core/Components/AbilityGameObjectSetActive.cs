using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	[AddComponentMenu(nameof(AbilityGameObjectSetActive))]
	public class AbilityGameObjectSetActive : MonoBehaviour, IActorAbility
	{
		public bool State;

		public GameObject[] Targets;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Execute()
		{
			for (var i = 0; i < Targets.Length; i++)
			{
				Targets[i].SetActive(State);
			}
		}
	}
}