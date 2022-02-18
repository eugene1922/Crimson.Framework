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
	public class PerkTeleport : CooldownBehaviour, IActorAbility, IPerkAbility, IPerkAbilityBindable, ILevelable,
		ICooldownable, IAimable
	{
		public AimingAnimationProperties aimingAnimProperties;
		public bool aimingAvailable;
		public AimingProperties aimingProperties;
		public float cooldownTime;
		public bool deactivateAimingOnCooldown;

		[Space]
		[TitleGroup("Levelable properties")]
		[OnValueChanged("SetLevelableProperty")]
		public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

		[ReadOnly] public int perkLevel = 1;

		public List<MonoBehaviour> perkRelatedComponents = new List<MonoBehaviour>();
		[LevelableValue] public float teleportDistance;
		private bool _aimingActive;
		private EntityManager _dstManager;
		private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();
		private IActor _target;
		private Vector3 _teleportVector = new Vector3();
		public bool ActionExecutionAllowed { get; set; }
		public IActor Actor { get; set; }

		public AimingAnimationProperties AimingAnimProperties
		{
			get => aimingAnimProperties;
			set => aimingAnimProperties = value;
		}

		public bool AimingAvailable
		{
			get => aimingAvailable;
			set => aimingAvailable = value;
		}

		public AimingProperties AimingProperties
		{
			get => aimingProperties;
			set => aimingProperties = value;
		}

		public int BindingIndex { get; set; } = -1;

		public float CooldownTime
		{
			get => cooldownTime;
			set => cooldownTime = value;
		}

		public bool DeactivateAimingOnCooldown
		{
			get => deactivateAimingOnCooldown;
			set => deactivateAimingOnCooldown = value;
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

		public bool OnHoldAttackActive { get; set; }

		public List<MonoBehaviour> PerkRelatedComponents
		{
			get
			{
				perkRelatedComponents.RemoveAll(c => c is null);
				return perkRelatedComponents;
			}
			set => perkRelatedComponents = value;
		}

		public GameObject SpawnedAimingPrefab { get; set; }

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

			if (!Actor.Abilities.Contains(this))
			{
				Actor.Abilities.Add(this);
			}
		}

		public void Apply(IActor target)
		{
			_target = target;

			ApplyActionWithCooldown(cooldownTime, ApplyTeleportPerk);
		}

		public void ApplyTeleportPerk()
		{
			if (!_target.AppliedPerks.Contains(this))
			{
				_target.AppliedPerks.Add(this);
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

		public void EvaluateAim(Vector2 pos)
		{
			_aimingActive = true;
			this.EvaluateAim(Actor as Actor, pos);
		}

		public void EvaluateAimByArea(Vector2 pos)
		{
			_teleportVector = Quaternion.Euler(0, -180, 0) * AbilityUtils.EvaluateAimByArea(this, pos);
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

		public void EvaluateAimBySight(Vector2 pos)
		{
			this.ResetAiming(Actor);
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
			if (Actor.AppliedPerks.Contains(this))
			{
				Actor.AppliedPerks.Remove(this);
			}

			foreach (var component in perkRelatedComponents)
			{
				Destroy(component);
			}

			Destroy(this);
		}

		public void ResetAiming()
		{
			this.ResetAiming(Actor);
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
	}
}