using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.Perks
{
	[HideMonoScript]
	public class PerkPeriodicDamage : TimerBaseBehaviour, IActorAbilityTarget, IPerkAbility, ILevelable
	{
		[LevelableValue] public float applyPeriod = 1;
		[LevelableValue] public float healthDecrement = 10;

		[Space]
		[TitleGroup("Levelable properties")]
		[OnValueChanged(nameof(SetLevelableProperty))]
		public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

		[ShowIf("limitedLifespan")]
		[LevelableValue] public float lifespan = 5;

		public bool limitedLifespan = true;
		[ReadOnly] public int perkLevel = 1;
		private IActor _effectInstance;
		private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();
		public IActor AbilityOwnerActor { get; set; }
		public IActor Actor { get; set; }

		public int Level
		{
			get => perkLevel;
			set => perkLevel = value;
		}

		public List<FieldInfo> LevelablePropertiesInfoCached
		{
			get
			{
				if (_levelablePropertiesInfoCached.Count == 0)
				{
					_levelablePropertiesInfoCached = this.GetFieldsWithAttributeInfo<LevelableValue>();
				}
				return _levelablePropertiesInfoCached;
			}
		}

		public List<LevelableProperties> LevelablePropertiesList
		{
			get => levelablePropertiesList;
			set => levelablePropertiesList = value;
		}

		public IActor TargetActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;

			if (!Actor.Abilities.Contains(this))
			{
				Actor.Abilities.Add(this);
			}
		}

		public void Apply(IActor target)
		{
			var copy = target.GameObject.CopyComponent(this) as PerkPeriodicDamage;

			if (copy == null)
			{
				Debug.LogError("[PERK PERIODIC DAMAGE] Error copying perk to Actor!");
				return;
			}
			var e = target.ActorEntity;
			copy.AddComponentData(ref e, target);
			if (!Actor.Spawner.AppliedPerks.Contains(copy))
			{
				Actor.Spawner.AppliedPerks.Add(copy);
			}

			copy.AbilityOwnerActor = this.Actor.Owner;
			copy.TargetActor = Actor.Spawner;
			copy._effectInstance = Actor;
			var targetActorEntity = target.ActorEntity;
			copy.AddComponentData(ref targetActorEntity, target);
			copy.Execute();
		}

		public void Execute()
		{
			if (TargetActor != Actor.Owner)
			{
				var ownerActorPlayer =
					Actor.Owner.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

				if (ownerActorPlayer == null)
				{
					return;
				}

				this.SetAbilityLevel(ownerActorPlayer.Level, LevelablePropertiesInfoCached, Actor, TargetActor);
				TryUpdateLifespan();
			}

			ApplyPeriodicDamage();

			if (!limitedLifespan)
			{
				return;
			}

			Timer.TimedActions.AddAction(Remove, lifespan);
		}

		public void Remove()
		{
			if (Actor == null)
			{
				return;
			}

			if (this.ContainsAction(ApplyPeriodicDamage))
			{
				this.RemoveAction(ApplyPeriodicDamage);
			}

			Destroy(this);
			if (Actor.AppliedPerks.Contains(this))
			{
				Actor.AppliedPerks.Remove(this);
			}
		}

		public void SetLevel(int level)
		{
			this.SetAbilityLevel(level, LevelablePropertiesInfoCached, Actor);
		}

		public void SetLevelableProperty()
		{
			this.SetLevelableProperty(LevelablePropertiesInfoCached);
		}

		private void ApplyPeriodicDamage()
		{
			if (TargetActor == null || Timer == null)
			{
				return;
			}

			TargetActor.ActorEntity.Damage(AbilityOwnerActor.ActorEntity, healthDecrement);

			Timer.TimedActions.AddAction(ApplyPeriodicDamage, applyPeriod);
		}

		private void TryUpdateLifespan()
		{
			var lifespanAbility = Actor.Abilities.FirstOrDefault(a => a is AbilityLifespan) as AbilityLifespan;
			if (lifespanAbility == null)
			{
				return;
			}

			lifespanAbility.lifespan = lifespan;
			lifespanAbility.Timer.TimedActions.Clear();
			lifespanAbility.Execute();
		}
	}
}