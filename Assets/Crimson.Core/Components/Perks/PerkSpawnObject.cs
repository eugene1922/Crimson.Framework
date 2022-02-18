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
	public class PerkSpawnObject : TimerBaseBehaviour, IActorAbility, IPerkAbility, IPerkAbilityBindable, ILevelable
	{
		[Space]
		[OnValueChanged("SetLevelableProperty")]
		public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

		[ReadOnly] public int perkLevel = 1;

		public List<MonoBehaviour> perkRelatedComponents = new List<MonoBehaviour>();
		private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();
		private IActor _target;
		public IActor Actor { get; set; }

		public int BindingIndex { get; set; } = -1;

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

		public List<MonoBehaviour> PerkRelatedComponents
		{
			get
			{
				perkRelatedComponents.RemoveAll(c => c is null);
				return perkRelatedComponents;
			}
			set => perkRelatedComponents = value;
		}

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
			_target = target;

			if (target != Actor.Owner)
			{
				var ownerActorPlayer =
					Actor.Owner.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

				if (ownerActorPlayer == null)
				{
					return;
				}

				this.SetAbilityLevel(ownerActorPlayer.Level, LevelablePropertiesInfoCached, Actor, _target);
			}

			if (!_target.AppliedPerks.Contains(this))
			{
				_target.AppliedPerks.Add(this);
			}
		}

		public void Execute()
		{
		}

		public void Remove()
		{
			if (_target != null && _target.AppliedPerks.Contains(this))
			{
				_target.AppliedPerks.Remove(this);
			}

			foreach (var component in perkRelatedComponents)
			{
				Destroy(component);
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