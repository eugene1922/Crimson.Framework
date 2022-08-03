using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
	public class AbilityDestroyTargetActor : MonoBehaviour, IActorAbilityTarget
	{
		private Entity _entity;

		public IActor Actor { get; set; }
		public IActor TargetActor { get; set; }
		public IActor AbilityOwnerActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;
		}

		public void Execute()
		{
			if (TargetActor == null)
			{
				return;
			}

			this.gameObject.DestroyWithEntity(_entity);
		}
	}
}