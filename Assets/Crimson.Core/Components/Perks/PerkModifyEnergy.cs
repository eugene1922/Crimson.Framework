using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Perks
{
	[HideMonoScript]
	public class PerkModifyEnergy : MonoBehaviour, IActorAbility, IPerkAbility, ILevelable
	{
		[Space]
		[TitleGroup("Levelable properties")]
		[OnValueChanged("SetLevelableProperty")]
		public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

		[ReadOnly] public int perkLevel = 1;
		[LevelableValue] public int Value = 15;
		private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();
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

		List<LevelableProperties> ILevelable.LevelablePropertiesList { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Apply(IActor target)
		{
			var copy = target.GameObject.CopyComponent(this) as PerkModifyEnergy;

			if (copy == null)
			{
				Debug.LogError("[PERK MODIFY ENERGY] Error copying perk to Actor!");
				return;
			}

			var e = target.ActorEntity;
			copy.AddComponentData(ref e, target);

			if (!Actor.Spawner.AppliedPerks.Contains(copy))
			{
				Actor.Spawner.AppliedPerks.Add(copy);
			}

			copy.Execute();
		}

		public void Execute()
		{
			var abilityActorPlayer = Actor.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

			if (abilityActorPlayer == null)
			{
				return;
			}

			abilityActorPlayer.UpdateEnergy(Value);
		}

		public void Remove()
		{
			var player = GetComponent<AbilityActorPlayer>();
			if (player != null)
			{
				player.UpdateEnergy(-Value);
			}

			Destroy(this);
		}

		public void SetLevel(int level)
		{
			this.SetAbilityLevel(level, LevelablePropertiesInfoCached, Actor);
		}

		public void SetLevelableProperty()
		{
			this.SetLevelableProperty(LevelablePropertiesInfoCached);
		}
	}
}