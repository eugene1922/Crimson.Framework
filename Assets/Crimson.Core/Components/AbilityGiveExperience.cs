using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
	[HideMonoScript]
	public class AbilityGiveExperience : MonoBehaviour, IActorAbilityTarget, ILevelable
	{
		[LevelableValue] public float ExpToGive = 0;

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

		public void Execute()
		{
			if (TargetActor == null)
			{
				return;
			}

			var targetActorPlayer = TargetActor.Abilities.Find(a => a is AbilityActorPlayer) as AbilityActorPlayer;

			if (targetActorPlayer == null)
			{
				return;
			}

			this.SetAbilityLevel(targetActorPlayer.Level, LevelablePropertiesInfoCached, Actor, TargetActor);

			targetActorPlayer.UpdateExperience((int)ExpToGive);
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