using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Enums;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
	public struct ActorDeathAnimData : IComponentData
	{
		public int AnimHash;
	}

	public struct ActorForceAnimData : IComponentData
	{
		public int AnimHash;
	}

	public struct ActorStafeAnimData : IComponentData
	{
		public int AnimHash;
	}

	public struct ActorTakeDamageAnimData : IComponentData
	{
		public int AnimHash;
	}

	public struct AdditionalForceActorTag : IComponentData
	{ }

	public struct DamagedActorTag : IComponentData
	{
	}

	public struct DeadActorTag : IComponentData
	{
	}

	public struct DestructionPendingTag : IComponentData
	{
	}

	public struct ImmediateDestructionActorTag : IComponentData
	{
	}

	public struct PerksSelectionAvailableTag : IComponentData
	{
	}

	public struct PlayerStateData : IComponentData
	{
		public int CurrentEnergy;
		public int CurrentExperience;
		public int CurrentHealth;
		public int Level;
		public int LevelUpRequiredExperience;
		public int MaxEnergy;
		public int MaxHealth;
		public int TotalDamageApplied;
	}

	public struct StrafeActorTag : IComponentData
	{ }

	[HideMonoScript]
	[NetworkSimObject]
	public class AbilityActorPlayer : MonoBehaviour, IActorAbility, ITimer, ILevelable
	{
		[TitleGroup("Player animation properties")]
		public ActorDeathAnimProperties actorDeathAnimProperties;

		public ActorTakeDamageAnimProperties actorTakeDamageAnimProperties;
		[HideInInspector] public bool actorToUI;
		public ActorGeneralAnimProperties additionalForceAnim;
		public float corpseCleanupDelay;

		[NetworkSimData]
		[ReadOnly]
		[CastToUI(nameof(CurrentEnergy))]
		public int CurrentEnergy;

		[NetworkSimData]
		[ReadOnly]
		[CastToUI(nameof(CurrentExperience))]
		public int CurrentExperience;

		[NetworkSimData]
		[ReadOnly]
		[CastToUI(nameof(CurrentHealth))]
		[LevelableValue]
		public int CurrentHealth;

		[NetworkSimData]
		[ReadOnly]
		[CastToUI(nameof(CurrentLevel))]
		public int CurrentLevel = 1;

		public DeadBehaviour deadActorBehaviour = new DeadBehaviour();

		[ReadOnly]
		public int deathCount;

		[NetworkSimData]
		[ReadOnly]
		[CastToUI(nameof(Energy))]
		[LevelableValue]
		public float Energy;

		[TitleGroup("UI channel info")]
		[OnValueChanged(nameof(UpdateUIChannelInfo))]
		public bool ExplicitUIChannel;

		[ValidateInput(nameof(MustBeAbility), "Ability MonoBehaviours must derive from IActorAbility!")]
		public MonoBehaviour healAction;

		[HideInInspector] public bool isEnabled = true;

		[Space]
		[TitleGroup("Levelable properties")]
		[OnValueChanged(nameof(SetLevelableProperty))]
		public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

		[TitleGroup("External behaviours")]
		[ValidateInput(nameof(MustBeAbility), "Ability MonoBehaviours must derive from IActorAbility!")]
		public MonoBehaviour levelUpAction;

		[NetworkSimData]
		[ReadOnly]
		[CastToUI(nameof(LevelUpRequiredExperience))]
		[LevelableValue]
		public int LevelUpRequiredExperience;

		[NetworkSimData]
		[ReadOnly]
		[CastToUI(nameof(MaxEnergy))]
		[LevelableValue]
		public int MaxEnergy;

		[NetworkSimData]
		[CastToUI(nameof(MaxHealth))]
		[LevelableValue]
		public int MaxHealth;

		[TitleGroup("Player data")]
		[NetworkSimData]
		[CastToUI("Name")]
		[InfoBox("32 symbols max")]
		public string PlayerName;

		public string targetMarkActorComponentName;

		[NetworkSimData]
		[ReadOnly]
		[CastToUI(nameof(TotalDamageApplied))]
		public int TotalDamageApplied;

		[ShowIf(nameof(ExplicitUIChannel))] public int UIChannelID = 0;

		[HideInInspector] public List<IActor> UIReceiverList = new List<IActor>();

		private EntityManager _dstManager;

		private Entity _entity;

		private Dictionary<string, FieldInfo> _fieldsInfo = new Dictionary<string, FieldInfo>();

		private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();

		private IActorAbility _maxDistanceWeapon;

		private TimerComponent _timer;

		public IActor Actor { get; set; }

		[ShowInInspector] public int ActorId => Actor?.ActorId ?? 0;

		public bool IsAlive => CurrentHealth > 0;

		public int Level
		{
			get => CurrentLevel;
			set => CurrentLevel = value;
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

		public IActorAbility MaxDistanceWeapon
		{
			get
			{
				return !ReferenceEquals(_maxDistanceWeapon, null)
					? _maxDistanceWeapon
					: Actor.Abilities.Where(a => a is AbilityWeapon)
					.OrderByDescending(w => ((AbilityWeapon)w).findTargetProperties.maxDistanceThreshold)
					.FirstOrDefault();
			}
		}

		public TimerComponent Timer => _timer = this.gameObject.GetOrCreateTimer(_timer);

		public bool TimerActive
		{
			get => isEnabled;
			set => isEnabled = value;
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;
			Actor.Owner = Actor;

			_fieldsInfo = new Dictionary<string, FieldInfo>();

			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			CurrentHealth = MaxHealth;
			LevelUpRequiredExperience = GameMeta.PointsToLevelUp;

			UIReceiverList = new List<IActor>();

			_dstManager.AddComponentData(entity, new PlayerStateData
			{
				CurrentHealth = CurrentHealth,
				MaxHealth = MaxHealth,
				CurrentExperience = CurrentExperience,
				LevelUpRequiredExperience = LevelUpRequiredExperience,
				Level = CurrentLevel
			});

			_timer = this.gameObject.GetOrCreateTimer(_timer);

			_dstManager.AddComponent<TimerData>(entity);

			if (actorTakeDamageAnimProperties.HasActorTakeDamageAnimation)
			{
				_dstManager.AddComponentData(entity, new ActorTakeDamageAnimData
				{
					AnimHash = Animator.StringToHash(actorTakeDamageAnimProperties.ActorTakeDamageName)
				});
			}

			if (additionalForceAnim.HasAnimation)
			{
				_dstManager.AddComponentData(entity, new ActorForceAnimData
				{
					AnimHash = additionalForceAnim.AnimationHash
				});
			}

			if (actorDeathAnimProperties.HasActorDeathAnimation)
			{
				_dstManager.AddComponentData(entity, new ActorDeathAnimData
				{
					AnimHash = Animator.StringToHash(actorDeathAnimProperties.ActorDeathAnimationName)
				});
			}

			foreach (var fieldInfo in typeof(AbilityActorPlayer).GetFields()
				.Where(field => field.GetCustomAttribute<CastToUI>(false) != null))
			{
				_fieldsInfo.Add(fieldInfo.Name, fieldInfo);
			}

			var playerInput = GetComponent<AbilityPlayerInput>();

			actorToUI = playerInput != null && playerInput.inputSource == InputSource.UserInput;

			if (!actorToUI)
			{
				return;
			}

			_dstManager.AddComponent<ApplyPresetPerksData>(Actor.ActorEntity);
		}

		public void Execute()
		{
		}

		public void FinishTimer()
		{
			_dstManager.AddComponent<ImmediateDestructionActorTag>(_entity);
		}

		public void ForceUpdatePlayerUIData()
		{
			foreach (var field in _fieldsInfo)
			{
				UpdateUIData(field.Key);
			}
		}

		public void LevelUp()
		{
			CurrentExperience -= LevelUpRequiredExperience;

			if (levelUpAction != null)
			{
				((IActorAbility)levelUpAction).Execute();
			}

			if (actorToUI)
			{
				SetLevel(Level + 1);
				var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);
				playerState.Level = Level;
				_dstManager.SetComponentData(_entity, playerState);
				_dstManager.AddComponent<PerksSelectionAvailableTag>(_entity);
			}
			else
			{
				Level++;
			}

			UpdateUIData(nameof(CurrentLevel));
		}

		public void SetLevel(int level)
		{
			this.SetAbilityLevel(level, LevelablePropertiesInfoCached, Actor);

			foreach (var ability in Actor.Abilities.Where(a => a is ILevelable && !ReferenceEquals(a, this)))
			{
				((ILevelable)ability).SetLevel(Level);
			}
		}

		public void SetLevelableProperty()
		{
			this.SetLevelableProperty(LevelablePropertiesInfoCached);
		}

		public void StartDeathTimer()
		{
			for (var i = 0; i < UIReceiverList.Count; i++)
			{
				var element = UIReceiverList[i];
				_dstManager.AddComponent<ImmediateDestructionActorTag>(element.ActorEntity);
			}

			StartTimer();
		}

		public void StartTimer()
		{
			Timer.TimedActions.AddAction(FinishTimer, corpseCleanupDelay);
		}

		public void UpdateEnergy(int delta)
		{
			if (!IsAlive)
			{
				return;
			}

			var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);
			var newValue = playerState.CurrentEnergy + delta;

			playerState.CurrentEnergy = Mathf.Clamp(newValue, 0, playerState.MaxEnergy);

			CurrentEnergy = playerState.CurrentEnergy;

			_dstManager.SetComponentData(_entity, playerState);

			UpdateUIData(nameof(CurrentEnergy));
		}

		public void UpdateExperienceData(int delta)
		{
			if (delta == 0)
			{
				return;
			}

			var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);

			CurrentExperience += delta;

			while (CurrentExperience >= LevelUpRequiredExperience)
			{
				LevelUp();
			}

			playerState.CurrentExperience = CurrentExperience;
			_dstManager.SetComponentData(_entity, playerState);

			UpdateUIData(nameof(LevelUpRequiredExperience));
			UpdateUIData(nameof(CurrentExperience));
		}

		public void UpdateHealth(int delta)
		{
			if (delta == 0)
			{
				_dstManager.AddComponent<DeadActorTag>(Actor.ActorEntity);
				return;
			}

			var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);
			var newValue = playerState.CurrentHealth + delta;

			playerState.CurrentHealth = Mathf.Clamp(newValue, 0, playerState.MaxHealth);

			CurrentHealth = playerState.CurrentHealth;

			_dstManager.SetComponentData(_entity, playerState);

			if (delta > 0 && healAction != null)
			{
				((IActorAbility)healAction).Execute();
			}

			UpdateUIData(nameof(CurrentHealth));

			if (!IsAlive)
			{
				deathCount++;
				_dstManager.AddComponent<DeadActorTag>(Actor.ActorEntity);
			}
		}

		public void UpdateMaxEnergy(int delta)
		{
			if (!IsAlive)
			{
				return;
			}

			var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);

			MaxEnergy += delta;
			CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
			playerState.CurrentEnergy = CurrentEnergy;
			playerState.MaxEnergy = MaxEnergy;

			_dstManager.SetComponentData(_entity, playerState);

			UpdateUIData(nameof(CurrentEnergy));
			UpdateUIData(nameof(MaxEnergy));
		}

		public void UpdateMaxHealthData(int delta)
		{
			if (delta == 0)
			{
				return;
			}

			var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);

			MaxHealth += delta;
			MaxHealth = playerState.MaxHealth;

			CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

			playerState.CurrentHealth = CurrentHealth;
			playerState.MaxHealth = MaxHealth;

			_dstManager.SetComponentData(_entity, playerState);

			UpdateUIData(nameof(CurrentHealth));
			UpdateUIData(nameof(MaxHealth));
		}

		public void UpdateTotalDamageData(int delta)
		{
			if (delta == 0)
			{
				return;
			}

			var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);

			playerState.TotalDamageApplied += delta;
			TotalDamageApplied = playerState.TotalDamageApplied;
			_dstManager.SetComponentData(_entity, playerState);

			UpdateUIData(nameof(TotalDamageApplied));
		}

		internal void UpdateMaxEnergy(float value)
		{
			throw new System.NotImplementedException();
		}

		private bool MustBeAbility(MonoBehaviour a)
		{
			return (a is IActorAbility) || (a is null);
		}

		private void UpdateUIChannelInfo()
		{
			if (!ExplicitUIChannel)
			{
				UIChannelID = 0;
			}
		}

		private void UpdateUIData(string fieldName)
		{
			foreach (var receiver in UIReceiverList.Where(receiver => _fieldsInfo.ContainsKey(fieldName)))
			{
				((UIReceiver)receiver)?.UpdateUIElementsData(
					_fieldsInfo[fieldName].GetCustomAttribute<CastToUI>(false).FieldId,
					_fieldsInfo[fieldName].GetValue(this));
			}
		}
	}
}