using Assets.Crimson.Core.Components.Weapons;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Loading;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.AbilityReactive
{
	[HideMonoScript]
	public class ThrowableWeapon : TimerBaseBehaviour,
		IThrowable,
		IActorSpawnerAbility,
		IComponentName,
		IEnableable,
		ICooldownable
	{
		public ActorProjectileSpawnAnimProperties actorProjectileSpawnAnimProperties;

		[HideIf(nameof(projectileClipCapacity), 0f)]
		[Space]
		public List<MonoBehaviour> clipReloadDisplayToggle = new List<MonoBehaviour>();

		public string componentName = "";

		[InfoBox("Clip Capacity of 0 stands for unlimited clip")]
		public int projectileClipCapacity = 0;

		public ActorSpawnerSettings projectileSpawnData;
		public Vector3 SpawnPointOffset;
		protected Entity _entity;
		private bool _actorToUi;
		[SerializeField] private float _cooldownTime = 0.3f;
		private EntityManager _dstManager;
		private int _projectileClip;
		public bool ActionExecutionAllowed { get; set; }
		public IActor Actor { get; set; }

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
		public bool IsEnable { get; set; }
		public List<Action<GameObject>> SpawnCallbacks { get; set; }
		public List<GameObject> SpawnedObjects { get; private set; } = new List<GameObject>();
		protected EntityManager CurrentEntityManager => World.DefaultGameObjectInjectionWorld.EntityManager;
		private Transform SpawnPointsRoot { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entity = entity;
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			SpawnCallbacks = new List<Action<GameObject>>();
			IsEnable = true;
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
			// ReSharper disable once CompareOfFloatsByEqualityOperator Here we need exact comparison
			if (IsEnable && _projectileClip > 0 && CurrentEntityManager.Exists(_entity))
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
			IsEnable = true;

			this.FinishAbilityCooldownTimer(Actor);
		}

		public void InitPool()
		{
			projectileSpawnData.InitPool();
		}

		public void ResetSpawnPointRootRotation()
		{
			SpawnPointsRoot.localRotation = Quaternion.identity;
		}

		public void Spawn()
		{
			SpawnedObjects = ActorSpawn.Spawn(projectileSpawnData, Actor, Actor.Owner);
			if (SpawnedObjects == null)
			{
				return;
			}

			var objectsToSpawn = SpawnedObjects;
			foreach (var obj in objectsToSpawn)
			{
				obj.transform.position = transform.TransformPoint(SpawnPointOffset);
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
		}

		public override void StartTimer()
		{
			IsEnable = false;
			base.StartTimer();

			this.StartAbilityCooldownTimer(Actor);
		}

		public void Throw()
		{
			Execute();
		}

#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawLine(Vector3.zero, SpawnPointOffset);
		}

#endif
	}
}