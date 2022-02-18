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
	public class AbilityModifyCurrentHealth : MonoBehaviour, IActorAbilityTarget, IPerkAbility, ILevelable
	{
		[LevelableValue] public float healthModifier = 15;

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
		}

		public void Apply(IActor target)
		{
			var copy = target.GameObject.CopyComponent(this) as AbilityModifyCurrentHealth;

			if (copy == null)
			{
				Debug.LogError("[PERK CURRENT MAX HEALTH] Error copying perk to Actor!");
				return;
			}
			var e = target.ActorEntity;
			copy.AddComponentData(ref e, target);
			copy.Execute();
		}

		public void Execute()
		{
			var actor = TargetActor ?? Actor;

			var abilityActorPlayer = actor.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

			if (abilityActorPlayer == null)
			{
				return;
			}

			abilityActorPlayer.UpdateHealth((int)healthModifier);
		}

		public void Remove()
		{
			var player = GetComponent<AbilityActorPlayer>();
			player.UpdateHealth((int)-healthModifier);

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