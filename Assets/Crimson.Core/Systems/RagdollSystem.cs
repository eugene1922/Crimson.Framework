using Assets.Crimson.Core.Components;
using Crimson.Core.Utils.LowLevel;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Systems
{
	[UpdateInGroup(typeof(FixedUpdateGroup))]
	public class RagdollSystem : ComponentSystem
	{
		private EntityQuery _actionQuery;

		protected override void OnCreate()
		{
			_actionQuery = GetEntityQuery(
				ComponentType.ReadOnly<Animator>(),
				ComponentType.ReadOnly<AbilityRagdoll>(),
				ComponentType.ReadOnly<ActivateRagdollData>());
		}

		protected override void OnUpdate()
		{
			var dstManager = World.EntityManager;
			Entities.With(_actionQuery).ForEach(
				(Entity entity, Animator animator, AbilityRagdoll abilityRagdoll) =>
				{
					dstManager.RemoveComponent<ActivateRagdollData>(entity);
					abilityRagdoll.Activate();
					animator.enabled = false;
				}
			);
		}
	}
}