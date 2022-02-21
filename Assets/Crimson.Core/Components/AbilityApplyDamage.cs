using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
	[HideMonoScript]
	public class AbilityApplyDamage : MonoBehaviour, IActorAbilityTarget, ILevelable
	{
		[LevelableValue, Range(0, 1)] public float CriticalDamageChance = .1f;
		[LevelableValue] public float CriticalDamageMultiplier = 1;
		[LevelableValue] public float Damage = 10;

		[Space]
		[TitleGroup("Levelable properties")]
		[OnValueChanged("SetLevelableProperty")]
		public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

		[ReadOnly] public int perkLevel = 1;
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
				return _levelablePropertiesInfoCached.Count > 0
					? _levelablePropertiesInfoCached
					: (_levelablePropertiesInfoCached = this.GetFieldsWithAttributeInfo<LevelableValue>());
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
		}

		public void Execute()
		{
			if (TargetActor == null)
			{
				return;
			}

			var ownerActorPlayer = AbilityOwnerActor.Abilities?.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;
			var targetActor = TargetActor.Abilities?.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

			if (ownerActorPlayer == null || targetActor == null)
			{
				return;
			}

			this.SetAbilityLevel(ownerActorPlayer.Level, LevelablePropertiesInfoCached, Actor, TargetActor);
			var damage = CalculateDamage(ownerActorPlayer);
			targetActor.UpdateHealth(-(int)damage);
		}

		public void SetLevel(int level)
		{
			this.SetAbilityLevel(level, LevelablePropertiesInfoCached, Actor);
		}

		public void SetLevelableProperty()
		{
			this.SetLevelableProperty(LevelablePropertiesInfoCached);
		}

		private float CalculateDamage(AbilityActorPlayer owner)
		{
			var damage = Damage;
			var randomValue = Random.Range(0.0f, 1.0f);
			var critChance = Mathf.Max(owner.Stats.CriticalDamageChance, CriticalDamageChance);
			var hasCritDamage = randomValue >= (1 - critChance);
			if (hasCritDamage)
			{
				var critMultiplier = owner.Stats.CriticalDamageMultiplier + CriticalDamageMultiplier;
				damage *= critMultiplier;
			}

			return damage;
		}
	}
}