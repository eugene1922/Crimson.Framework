using Crimson.Core.Common;
using Crimson.Core.Enums;
using Crimson.Core.Loading;
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
	public class PerkModifyMoveSpeed : CooldownBehaviour, IActorAbility, IPerkAbility, IPerkAbilityBindable,
		ILevelable, ICooldownable
	{
		public float cooldownTime;

		[Space]
		[TitleGroup("Levelable properties")]
		[OnValueChanged("SetLevelableProperty")]
		public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

		[ShowIf("limitedLifespan")]
		[LevelableValue]
		public int lifespan;

		public bool limitedLifespan;
		public GameObject moveFX;
		public float moveSpeedMultiplier = 1.5f;
		[ReadOnly] public int perkLevel = 1;
		public List<MonoBehaviour> perkRelatedComponents = new List<MonoBehaviour>();
		private EntityManager _dstManager;
		private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();
		private IActor _target;
		public IActor Actor { get; set; }

		public int BindingIndex { get; set; } = -1;

		public float CooldownTime
		{
			get => cooldownTime;
			set => cooldownTime = value;
		}

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

			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			if (!Actor.Abilities.Contains(this))
			{
				Actor.Abilities.Add(this);
			}
		}

		public void Apply(IActor target)
		{
			_target = target;

			ApplyActionWithCooldown(cooldownTime, ApplyModifyMoveSpeedPerk);
		}

		public void ApplyModifyMoveSpeedPerk()
		{
			if (_target == null || !_dstManager.HasComponent<ActorMovementData>(_target.ActorEntity))
			{
				return;
			}

			if (_target != Actor.Owner)
			{
				var ownerActorPlayer =
					Actor.Owner.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

				if (ownerActorPlayer == null)
				{
					return;
				}

				this.SetAbilityLevel(ownerActorPlayer.Level, LevelablePropertiesInfoCached, Actor, _target);
				TryUpdateLifespan();
			}

			if (!_target.AppliedPerks.Contains(this))
			{
				_target.AppliedPerks.Add(this);
			}

			if (moveFX != null)
			{
				var spawnData = new ActorSpawnerSettings
				{
					objectsToSpawn = new List<GameObject> { moveFX },
					SpawnPosition = SpawnPosition.UseSpawnerPosition,
					parentOfSpawns = TargetType.None,
					runSpawnActionsOnObjects = true,
					destroyAbilityAfterSpawn = true
				};

				var fx = ActorSpawn.Spawn(spawnData, Actor, null)?.First();
			}

			var movementData = _dstManager.GetComponentData<ActorMovementData>(_target.ActorEntity);
			movementData.ExternalMultiplier *= moveSpeedMultiplier;

			_dstManager.SetComponentData(_target.ActorEntity, movementData);

			if (!limitedLifespan)
			{
				return;
			}

			Timer.TimedActions.AddAction(FinishSpeedUpTimer, lifespan);
		}

		public void Execute()
		{
			Apply(Actor);
		}

		public override void FinishTimer()
		{
			base.FinishTimer();
			this.FinishAbilityCooldownTimer(Actor);
		}

		public void Remove()
		{
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

		public override void StartTimer()
		{
			base.StartTimer();
			this.StartAbilityCooldownTimer(Actor);
		}

		private void FinishSpeedUpTimer()
		{
			if (_target == null || !_dstManager.HasComponent<ActorMovementData>(_target.ActorEntity) || moveSpeedMultiplier <= 0)
			{
				return;
			}

			if (_target.AppliedPerks.Contains(this))
			{
				_target.AppliedPerks.Remove(this);
			}

			var movementData = _dstManager.GetComponentData<ActorMovementData>(_target.ActorEntity);

			movementData.ExternalMultiplier /= moveSpeedMultiplier;
			_dstManager.SetComponentData(_target.ActorEntity, movementData);
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