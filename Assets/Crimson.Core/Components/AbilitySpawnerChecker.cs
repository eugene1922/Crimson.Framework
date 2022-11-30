using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilitySpawnerChecker : MonoBehaviour, IActorAbility
	{
		[ValidateInput(nameof(MustBeAbility), "Execute abilities when spawner is disappeared")]
		public MonoBehaviour[] Actions;

		public bool DestroyOnExecute;

		private ActorAbilityList _actions;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_actions = new ActorAbilityList(Actions);
			Actor = actor;
		}

		public void Execute()
		{
			_actions?.Execute();
			if (DestroyOnExecute)
			{
				Destroy(this);
			}
		}

		private bool MustBeAbility(MonoBehaviour[] items)
		{
			return items == null || items.All(s => s is IActorAbility);
		}
	}
}