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
	public class PerkDash : CooldownBehaviour, IActorAbility, IPerkAbility, IPerkAbilityBindable, ILevelable,
		ICooldownable, IAimable
	{
		public AimingAnimationProperties aimingAnimProperties;
		public bool aimingAvailable;
		public AimingProperties aimingProperties;
		public float cooldownTime;
		public List<GameObject> dashFX = new List<GameObject>();
		public bool deactivateAimingOnCooldown;
		[LevelableValue] public float force = 25;

		[Space]
		[TitleGroup("Levelable properties")]
		[OnValueChanged("SetLevelableProperty")]
		public List<LevelableProperties> levelablePropertiesList = new List<LevelableProperties>();

		[Sirenix.OdinInspector.ReadOnly] public int perkLevel = 1;
		public List<MonoBehaviour> perkRelatedComponents = new List<MonoBehaviour>();
		public float timer = 0.1f;
		private bool _aimingActive;
		private Vector3 _dashVector = new Vector3();
		private EntityManager _dstManager;
		private List<FieldInfo> _levelablePropertiesInfoCached = new List<FieldInfo>();
		private Vector3 _previousVelocity;
		private IActor _target;
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

			ApplyActionWithCooldown(cooldownTime, ApplyDash);
		}

		public void ApplyDash()
		{
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

			if (!_target.AppliedPerks.Contains(this))
			{
				_target.AppliedPerks.Add(this);
			}

			var targetRigidbody = _target.GameObject.GetComponent<Rigidbody>();

			if (targetRigidbody == null)
			{
				return;
			}

			_previousVelocity = targetRigidbody.velocity;

			var dashVector = new Vector3();

			if (_aimingActive)
			{
				switch (AimingProperties.aimingType)
				{
					case AimingType.AimingArea:
						dashVector = _dashVector;
						break;

					case AimingType.SightControl:
						transform.LookAt(SpawnedAimingPrefab.transform);
						dashVector = _target.GameObject.transform.forward;
						break;
				}
			}
			else
			{
				dashVector = _target.GameObject.transform.forward;
			}
			var movement = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ActorMovementData>(_target.ActorEntity);
			movement.Input = dashVector * force;
			World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(_target.ActorEntity, movement);

			targetRigidbody.AddForce(dashVector * force, ForceMode.Impulse);

			if (dashFX != null && dashFX.Count > 0)
			{
				var spawnData = new ActorSpawnerSettings
				{
					objectsToSpawn = dashFX,
					SpawnPosition = SpawnPosition.UseSpawnerPosition,
					parentOfSpawns = TargetType.None,
					runSpawnActionsOnObjects = true,
					destroyAbilityAfterSpawn = true
				};

				var fx = ActorSpawn.Spawn(spawnData, Actor, null)?.First();
			}

			Timer.TimedActions.AddAction(() =>
			{
				if (targetRigidbody == null)
				{
					return;
				}

				targetRigidbody.velocity = _previousVelocity;
				_dashVector = Vector3.zero;
			}, timer);

			_aimingActive = false;
		}

		public void EvaluateAim(Vector2 pos)
		{
			_aimingActive = true;
			this.EvaluateAim(Actor as Actor, pos);
		}

		public void EvaluateAimByArea(Vector2 pos)
		{
			_dashVector = Quaternion.Euler(0, -180, 0) * AbilityUtils.EvaluateAimByArea(this, pos);
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
			this.EvaluateAimBySight(Actor, pos);
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