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

    public struct AdditionalForceActorData : IComponentData
    { }

    public struct DamagedActorData : IComponentData
    {
    }

    public struct DeadActorData : IComponentData
    {
    }

    public struct DestructionPendingData : IComponentData
    {
    }

    public struct ImmediateActorDestructionData : IComponentData
    {
    }

    public struct PerksSelectionAvailableData : IComponentData
    {
    }

    public struct PlayerStateData : IComponentData
    {
        public int CurrentExperience;
        public int CurrentHealth;
        public int Level;
        public int LevelUpRequiredExperience;
        public int MaxHealth;
        public int TotalDamageApplied;
    }

    public struct StrafeActorData : IComponentData
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
        [CastToUI("CurrentExperience")]
        public int CurrentExperience;

        [NetworkSimData]
        [ReadOnly]
        [CastToUI("CurrentHealth")]
        [LevelableValue]
        public int CurrentHealth;

        [NetworkSimData]
        [ReadOnly]
        [CastToUI("CurrentLevel")]
        public int CurrentLevel = 1;

        public DeadBehaviour deadActorBehaviour = new DeadBehaviour();

        [ReadOnly]
        public int deathCount;

        [TitleGroup("UI channel info")]
        [OnValueChanged("UpdateUIChannelInfo")]
        public bool ExplicitUIChannel;

        [ValidateInput("MustBeAbility", "Ability MonoBehaviours must derive from IActorAbility!")]
        public MonoBehaviour healAction;

        [HideInInspector] public bool isEnabled = true;

        [Space]
        [TitleGroup("Levelable properties")]
        [OnValueChanged("SetLevelableProperty")]
        public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

        [TitleGroup("External behaviours")]
        [ValidateInput("MustBeAbility", "Ability MonoBehaviours must derive from IActorAbility!")]
        public MonoBehaviour levelUpAction;

        [NetworkSimData]
        [ReadOnly]
        [CastToUI("LevelUpRequiredExperience")]
        [LevelableValue]
        public int LevelUpRequiredExperience;

        [NetworkSimData]
        [CastToUI("MaxHealth")]
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
        [CastToUI("TotalDamageApplied")]
        public int TotalDamageApplied;

        [ShowIf("ExplicitUIChannel")] public int UIChannelID = 0;
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
                if (_levelablePropertiesInfoCached.Any()) return _levelablePropertiesInfoCached;
                return _levelablePropertiesInfoCached = this.GetFieldsWithAttributeInfo<LevelableValue>();
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
                if (!ReferenceEquals(_maxDistanceWeapon, null)) return _maxDistanceWeapon;

                return Actor.Abilities.Where(a => a is AbilityWeapon)
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

            if (!actorToUI) return;

            _dstManager.AddComponent<ApplyPresetPerksData>(Actor.ActorEntity);
            //_dstManager.AddComponent<PerksSelectionAvailableData>(_entity);
        }

        public void Execute()
        {
        }

        public void FinishTimer()
        {
            _dstManager.AddComponent<ImmediateActorDestructionData>(_entity);
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
                _dstManager.AddComponent<PerksSelectionAvailableData>(_entity);
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
            foreach (var element in UIReceiverList)
            {
                _dstManager.AddComponent<ImmediateActorDestructionData>(element.ActorEntity);
            }

            StartTimer();
        }

        public void StartTimer()
        {
            Timer.TimedActions.AddAction(FinishTimer, corpseCleanupDelay);
        }

        public void UpdateExperienceData(int delta)
        {
            if (delta == 0) return;

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

        public void UpdateHealthData(int delta)
        {
            var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);

            if (delta == 0)
            {
                _dstManager.AddComponent<DeadActorData>(Actor.ActorEntity);
                return;
            }

            var newHealth = playerState.CurrentHealth + delta;

            playerState.CurrentHealth =
                newHealth < 0 ? 0 : newHealth > playerState.MaxHealth ? playerState.MaxHealth : newHealth;

            CurrentHealth = playerState.CurrentHealth;

            _dstManager.SetComponentData(_entity, playerState);

            if (delta > 0)
            {
                if (healAction != null) ((IActorAbility)healAction).Execute();
            }

            UpdateUIData(nameof(CurrentHealth));

            if (!IsAlive)
            {
                deathCount++;
                _dstManager.AddComponent<DeadActorData>(Actor.ActorEntity);
            }
        }

        public void UpdateMaxHealthData(int delta)
        {
            var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);

            if (delta == 0) return;

            playerState.MaxHealth += delta;
            MaxHealth = playerState.MaxHealth;

            _dstManager.SetComponentData(_entity, playerState);

            UpdateUIData(nameof(MaxHealth));
            UpdateUIData(nameof(CurrentHealth));
        }

        public void UpdateTotalDamageData(int delta)
        {
            if (delta == 0) return;

            var playerState = _dstManager.GetComponentData<PlayerStateData>(_entity);

            playerState.TotalDamageApplied += delta;
            TotalDamageApplied = playerState.TotalDamageApplied;
            _dstManager.SetComponentData(_entity, playerState);

            UpdateUIData(nameof(TotalDamageApplied));
        }

        private bool MustBeAbility(MonoBehaviour a)
        {
            return (a is IActorAbility) || (a is null);
        }

        private void UpdateUIChannelInfo()
        {
            if (!ExplicitUIChannel) UIChannelID = 0;
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