using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components.Tags;
using Assets.Crimson.Core.Components.Weapons;
using Crimson.Core.Common;
using Crimson.Core.Components.Interfaces;
using Crimson.Core.Enums;
using Crimson.Core.Loading;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crimson.Core.Components.AbilityReactive
{
	[HideMonoScript]
	public class WeaponSpawnOnes : TimerBaseBehaviour,
		IWeapon,
		IActorSpawnerAbility,
		IComponentName,
		ICooldownable,
		IBindable,
		IUseAimable
	{
		public ActorProjectileSpawnAnimProperties actorProjectileSpawnAnimProperties;

		[ValidateInput(nameof(MustBeAimable), "Ability MonoBehaviours must derive from IAimable!")]
		public MonoBehaviour AimComponent;

		[HideInInspector] public List<string> appliedPerksNames = new List<string>();

		[HideIf(nameof(projectileClipCapacity), 0f)]
		[Space]
		public List<MonoBehaviour> clipReloadDisplayToggle = new List<MonoBehaviour>();

		[HideIf(nameof(projectileClipCapacity), 0f)] public float clipReloadTime = 1f;
		public string componentName = "";

		[InfoBox("Clip Capacity of 0 stands for unlimited clip")]
		public int projectileClipCapacity = 0;

		public ActorSpawnerSettings projectileSpawnData;
		public ActorGeneralAnimProperties reloadAnimProps;

		[InfoBox("Put here IEnable implementation to display reload")]
		[Space]
		public List<MonoBehaviour> reloadDisplayToggle = new List<MonoBehaviour>();

		public bool suppressWeaponSpawn = false;
		protected Entity _entity;
		private bool _actorToUi;
		[SerializeField] private float _cooldownTime = 0.3f;
		private EntityManager _dstManager;
		private int _projectileClip;
		public bool ActionExecutionAllowed { get; set; }
		public IActor Actor { get; set; }
		public IAimable Aim => AimComponent as IAimable;
		public int BindingIndex { get; set; } = -1;

		public string ComponentName
		{
			get => componentName;
			set => componentName = value;
		}

		public float CooldownTime
		{
			get => _cooldownTime;
			set => _cooldownTime = value;
		}

		public Action<GameObject> DisposableSpawnCallback { get; set; }
		public bool IsActivated { get; set; }
		public bool OnHoldAttackActive { get; set; }
		public List<Action<GameObject>> SpawnCallbacks { get; set; }
		public GameObject SpawnedAimingPrefab { get; set; }
		public List<GameObject> SpawnedObjects { get; private set; } = new List<GameObject>();
		public bool IsEnable { get; set; }

		protected EntityManager CurrentEntityManager => World.DefaultGameObjectInjectionWorld.EntityManager;
		private Transform SpawnPointsRoot { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entity = entity;
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			SpawnCallbacks = new List<Action<GameObject>>();
			_projectileClip = projectileClipCapacity;
			_dstManager.AddComponent<TimerData>(entity);

			if (actorProjectileSpawnAnimProperties != null
				&& actorProjectileSpawnAnimProperties.HasActorProjectileAnimation)
			{
				_dstManager.AddComponentData(entity, new ActorProjectileAnimData
				{
					AnimHash = Animator.StringToHash(actorProjectileSpawnAnimProperties.ActorProjectileAnimationName)
				});
			}

			if (reloadAnimProps.HasAnimation)
			{
				_dstManager.AddComponentData(entity, new ReloadAnimationData
				{
					AnimHash = reloadAnimProps.AnimationHash
				});
			}

			appliedPerksNames = new List<string>();

			var playerActor = actor.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;
			_actorToUi = playerActor != null && playerActor.actorToUI;

			if (!Actor.Abilities.Contains(this))
			{
				Actor.Abilities.Add(this);
			}

			if (!_actorToUi)
			{
				return;
			}

			SpawnPointsRoot = new GameObject("spawn points root").transform;
			SpawnPointsRoot.SetParent(gameObject.transform);

			SpawnPointsRoot.localPosition = Vector3.zero;
			ResetSpawnPointRootRotation();

			if (projectileSpawnData.SpawnPosition == SpawnPosition.UseSpawnerPosition)
			{
				projectileSpawnData.SpawnPosition = SpawnPosition.UseSpawnPoints;
			}

			if (projectileSpawnData.SpawnPoints.Any())
			{
				projectileSpawnData.SpawnPoints.Clear();
			}

			var baseSpawnPoint = new GameObject("Base Spawn Point");
			baseSpawnPoint.transform.SetParent(SpawnPointsRoot);

			baseSpawnPoint.transform.localPosition = Vector3.zero;
			baseSpawnPoint.transform.localRotation = Quaternion.identity;

			projectileSpawnData.SpawnPoints.Add(baseSpawnPoint);

			InitPool();
		}

		public void Execute()
		{
			if (!IsEnable)
			{
				return;
			}

			// ReSharper disable once CompareOfFloatsByEqualityOperator Here we need exact comparison
			if (IsActivated && _projectileClip > 0 && CurrentEntityManager.Exists(_entity))
			{
				Spawn();

				CurrentEntityManager.AddComponentData(_entity,
					new ActorProjectileThrowAnimTag());

				_projectileClip--;
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (CooldownTime == 0)
				{
					return;
				}

				StartTimer();
				Timer.TimedActions.AddAction(FinishTimer, CooldownTime);
			}
		}

		public override void FinishTimer()
		{
			base.FinishTimer();
			this.FinishAbilityCooldownTimer(Actor);
		}

		public void InitPool()
		{
			projectileSpawnData.InitPool();
		}

		public void Reload()
		{
			CurrentEntityManager.AddComponentData(_entity, new ReloadTag());
			_projectileClip = projectileClipCapacity;
		}

		public void ResetSpawnPointRootRotation()
		{
			SpawnPointsRoot.localRotation = Quaternion.identity;
		}

		public void Spawn()
		{
			if (Aim != null && Aim.SpawnedAimingPrefab != null)
			{
				SpawnPointsRoot.LookAt(Aim.SpawnedAimingPrefab.transform);
			}
			SpawnedObjects = ActorSpawn.Spawn(projectileSpawnData, Actor, Actor.Owner);
			if (SpawnedObjects == null)
			{
				return;
			}

			var objectsToSpawn = SpawnedObjects;

			if (SpawnedObjects == null)
			{
				return;
			}

			foreach (var callback in SpawnCallbacks)
			{
				objectsToSpawn.ForEach(go => callback.Invoke(go));
			}

			objectsToSpawn.ForEach(go =>
			{
				DisposableSpawnCallback?.Invoke(go);
				DisposableSpawnCallback = null;
			});

			if (!_actorToUi)
			{
				return;
			}

			ResetSpawnPointRootRotation();
			OnHoldAttackActive = false;
		}

		public void StartFire()
		{
			IsActivated = true;
			Execute();
			IsActivated = false;
		}

		public override void StartTimer()
		{
			base.StartTimer();
			this.StartAbilityCooldownTimer(Actor);
		}

		public void StopFire()
		{
		}

		private bool MustBeAimable(MonoBehaviour behaviour)
		{
			return behaviour is IActorAbility;
		}
	}
}