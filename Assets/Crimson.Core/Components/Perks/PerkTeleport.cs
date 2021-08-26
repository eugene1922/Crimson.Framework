using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.Perks
{
    [HideMonoScript]
    public class PerkTeleport : CooldownBehaviour, IActorAbility, IPerkAbility, IPerkAbilityBindable, ILevelable,
        ICooldownable, IAimable
    {
        [ReadOnly] public int perkLevel = 1;

        [LevelableValue] public float teleportDistance;

        public float cooldownTime;

        public bool aimingAvailable;
        public bool deactivateAimingOnCooldown;
        
        public AimingProperties aimingProperties;
        public AimingAnimationProperties aimingAnimProperties;

        public List<MonoBehaviour> perkRelatedComponents = new List<MonoBehaviour>();

        [Space] [TitleGroup("Levelable properties")] [OnValueChanged("SetLevelableProperty")]
        public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

        public IActor Actor { get; set; }

        public List<MonoBehaviour> PerkRelatedComponents
        {
            get
            {
                perkRelatedComponents.RemoveAll(c => ReferenceEquals(c, null));
                return perkRelatedComponents;
            }
            set => perkRelatedComponents = value;
        }

        public int Level
        {
            get => perkLevel;
            set => perkLevel = value;
        }

        public List<LevelableProperties> LevelablePropertiesList
        {
            get => levelablePropertiesList;
            set => levelablePropertiesList = value;
        }

        public List<FieldInfo> LevelablePropertiesInfoCached
        {
            get
            {
                if (_levelablePropertiesInfoCached.Any()) return _levelablePropertiesInfoCached;
                return _levelablePropertiesInfoCached = this.GetFieldsWithAttributeInfo<LevelableValue>();
            }
        }

        public bool ActionExecutionAllowed { get; set; }
        public GameObject SpawnedAimingPrefab { get; set; }

        public int BindingIndex { get; set; } = -1;

        public float CooldownTime
        {
            get => cooldownTime;
            set => cooldownTime = value;
        }

        public bool AimingAvailable
        {
            get => aimingAvailable;
            set => aimingAvailable = value;
        }

        public bool DeactivateAimingOnCooldown
        {
            get => deactivateAimingOnCooldown;
            set => deactivateAimingOnCooldown = value;
        }

        public bool OnHoldAttackActive { get; set; }

        public AimingProperties AimingProperties
        {
            get => aimingProperties;
            set => aimingProperties = value;
        }

        public AimingAnimationProperties AimingAnimProperties
        {
            get => aimingAnimProperties;
            set => aimingAnimProperties = value;
        }

        private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();

        private IActor _target;
        private bool _aimingActive;
        private Vector3 _teleportVector = new Vector3();
        
        private EntityManager _dstManager;

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
            
            _dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            if (AimingAnimProperties.HasActorAimingAnimation)
            {
                _dstManager.AddComponentData(entity, new AimingAnimProperties
                {
                    AnimHash = Animator.StringToHash(AimingAnimProperties.ActorAimingAnimationName)
                });
            }

            if (!Actor.Abilities.Contains(this)) Actor.Abilities.Add(this);
        }

        public void Execute()
        {
            Apply(Actor);
        }

        public void Apply(IActor target)
        {
            _target = target;

            ApplyActionWithCooldown(cooldownTime, ApplyTeleportPerk);
        }

        public void ApplyTeleportPerk()
        {
            if (!_target.AppliedPerks.Contains(this)) _target.AppliedPerks.Add(this);

            if (_target != Actor.Owner)
            {
                var ownerActorPlayer =
                    Actor.Owner.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

                if (ownerActorPlayer == null) return;

                this.SetAbilityLevel(ownerActorPlayer.Level, LevelablePropertiesInfoCached, Actor, _target);
            }

            var teleportVector = new Vector3();

            if (_aimingActive)
            {
                switch (AimingProperties.aimingType)
                {
                    case AimingType.AimingArea:
                        teleportVector = _teleportVector;
                        break;
                    case AimingType.SightControl:
                        transform.LookAt(SpawnedAimingPrefab.transform);
                        teleportVector = _target.GameObject.transform.forward;
                        break;
                }
            }
            else
            {
                teleportVector = _target.GameObject.transform.forward;
            }

            _target.GameObject.transform.position += teleportVector * teleportDistance;

            _aimingActive = false;
        }

        public void SetLevel(int level)
        {
            this.SetAbilityLevel(level, LevelablePropertiesInfoCached, Actor);
        }

        public void Remove()
        {
            if (Actor.AppliedPerks.Contains(this)) Actor.AppliedPerks.Remove(this);

            foreach (var component in perkRelatedComponents)
            {
                Destroy(component);
            }

            Destroy(this);
        }

        public override void FinishTimer()
        {
            base.FinishTimer();
            this.FinishAbilityCooldownTimer(Actor);
        }

        public override void StartTimer()
        {
            base.StartTimer();
            this.StartAbilityCooldownTimer(Actor);
        }


        public void SetLevelableProperty()
        {
            this.SetLevelableProperty(LevelablePropertiesInfoCached);
        }

        public void EvaluateAim(Vector2 pos)
        {
            _aimingActive = true;
            this.EvaluateAim(Actor as Actor, pos);
        }
        
        public void EvaluateAimBySelectedType(Vector2 pos)
        {
            switch (AimingProperties.aimingType)
            {
                case AimingType.AimingArea:
                    EvaluateAimByArea(pos);
                    break;
                case AimingType.SightControl:
                    EvaluateAimBySight(pos);
                    break;
            }
        }

        public void EvaluateAimByArea(Vector2 pos)
        {
            _teleportVector = Quaternion.Euler(0, -180, 0) * AbilityUtils.EvaluateAimByArea(this, pos);
        }

        public void EvaluateAimBySight(Vector2 pos)
        {
            this.ResetAiming(Actor);
        }

        public void ResetAiming()
        {
            this.ResetAiming(Actor);
        }
    }
}