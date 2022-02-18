using Crimson.Core.Common;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
	public enum DuplicateHandling
	{
		Abort = 0,
		Replace = 1
	}

	[HideMonoScript]
	public class AbilityActorApplyPerk : TimerBaseBehaviour, IActorAbilityTarget
	{
		[EnumToggleButtons] public DuplicateHandling perkDuplicateHandling;

		[ValidateInput(nameof(MustBePerk), "Perk MonoBehaviours must derive from IPerkAbility!")]
		public MonoBehaviour perkToApply;

		public IActor AbilityOwnerActor { get; set; }
		public IActor Actor { get; set; }
		public IActor TargetActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Execute()
		{
			if (perkToApply == null || TargetActor == null)
			{
				return;
			}

			ApplyPerk();
		}

		private void ApplyPerk()
		{
			var existingPerk = TargetActor.AppliedPerks.Find(perk => perk.GetType() == perkToApply.GetType());

			if (existingPerk != null)
			{
				switch (perkDuplicateHandling)
				{
					case DuplicateHandling.Abort:
						return;

					case DuplicateHandling.Replace:
						if (existingPerk is TimerBaseBehaviour timerBaseBehaviour)
						{
							timerBaseBehaviour.ResetTimer();
							var perkActor = (existingPerk as IActorAbility)?.Actor;
							if (perkActor == null)
							{
								return;
							}

							TryResetPerkGameObjectLifespan(perkActor.GameObject);

							return;
						}

						existingPerk.Remove();
						break;
				}
			}

			var spawnAbility = gameObject.AddComponent<AbilityActorSimpleSpawn>();

			spawnAbility.Actor = TargetActor;

			spawnAbility.objectToSpawn = perkToApply.gameObject;
			spawnAbility.ownerType = OwnerType.CurrentActorOwner;
			spawnAbility.spawnerType = SpawnerType.CurrentActor;
			spawnAbility.DestroyAfterSpawn = true;

			spawnAbility.Execute();
		}

		private bool MustBePerk(MonoBehaviour perk)
		{
			return perk == null || perk is IPerkAbility;
		}

		private void TryResetPerkGameObjectLifespan(GameObject perkGameObject)
		{
			var abilityLifespan = perkGameObject.GetComponent<AbilityLifespan>();

			if (abilityLifespan == null)
			{
				return;
			}

			abilityLifespan.Timer.TimedActions.Clear();
			abilityLifespan.Execute();
		}
	}
}