using Crimson.Core.Common;
using Crimson.Core.Loading;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Crimson.Core.Components.AbilityReactive
{
	public class AutoAim : TimerBaseBehaviour,
		IActorAbility,
		IActorSpawnerAbility,
		IComponentName,
		IAimable
	{
		public AimingAnimationProperties aimingAnimProperties;
		public bool aimingAvailable;

		[Space,
		ShowInInspector]
		public string componentName = "";

		public bool deactivateAimingOnCooldown;
		public FindTargetProperties FindTargetProperties = new FindTargetProperties();
		public ActorSpawnerSettings MarkSpawnData;

		[InfoBox("Time when aim disable (Seconds)")]
		public float ResetAimTime = 0.15f;

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

		public virtual AimingProperties AimingProperties { get; set; }
		public int BindingIndex { get; set; } = -1;

		public string ComponentName
		{
			get => componentName;
			set => componentName = value;
		}

		public bool DeactivateAimingOnCooldown
		{
			get => deactivateAimingOnCooldown;
			set => deactivateAimingOnCooldown = value;
		}

		public Action<GameObject> DisposableSpawnCallback { get; set; }
		public Entity Entity { get; set; }
		public bool OnHoldAttackActive { get; set; }
		public List<Action<GameObject>> SpawnCallbacks { get; set; }
		public GameObject SpawnedAimingPrefab { get; set; }
		public List<GameObject> SpawnedObjects { get; private set; } = new List<GameObject>();
		public Transform SpawnPointsRoot { get; private set; }
		protected Entity CurrentEntity { get; private set; }
		protected EntityManager CurrentEntityManager { get; private set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Entity = entity;
			Actor = actor;

			CurrentEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			CurrentEntity = entity;

			SpawnCallbacks = new List<Action<GameObject>>();

			CurrentEntityManager.AddComponent<TimerData>(entity);

			if (MarkSpawnData.SpawnPoints != null
				&& MarkSpawnData.SpawnPoints.Count != 0)
			{
				MarkSpawnData.SpawnPoints.Clear();
			}
			InitPool();
		}

		public void EvaluateAim(Vector2 pos)
		{
			this.EvaluateAim(Actor as Actor, pos);
		}

		public void EvaluateAimByArea(Vector2 pos)
		{
			var aimPosition = AbilityUtils.EvaluateAimByArea(this, pos);

			DisposableSpawnCallback = go =>
			{
				var targetActor = go.GetComponent<Actor>();
				if (targetActor == null)
				{
					return;
				}

				var vector = Quaternion.Euler(0, -180, 0) * aimPosition;
			};
		}

		public void EvaluateAimBySelectedType(Vector2 pos)
		{
			EvaluateAimTarget(pos);
		}

		public void EvaluateAimBySight(Vector2 pos)
		{
			var aimPosition = this.EvaluateAimBySight(Actor, pos);

			DisposableSpawnCallback = go =>
			{
				var targetActor = go.GetComponent<Actor>();
				if (targetActor == null)
				{
					return;
				}

				var vector = aimPosition - Actor.GameObject.transform.position;

				CurrentEntityManager.AddComponentData(targetActor.ActorEntity, new DestroyProjectileInPointData
				{
					Point = new float2(aimPosition.x, aimPosition.z)
				});
			};
		}

		public void EvaluateAimTarget(Vector2 input)
		{
			switch (AimingProperties.aimingType)
			{
				case AimingType.AimingArea:
					EvaluateAimByArea(input);
					break;

				case AimingType.SightControl:
					EvaluateAimBySight(input);
					break;

				default:
					throw new NotImplementedException("No aim type");
			}
		}

		public void Execute()
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator Here we need exact comparison
			if (CurrentEntityManager.Exists(CurrentEntity))
			{
				Spawn();

				StartTimer();
				this.RestartAction(FinishTimer, ResetAimTime);
			}
		}

		public override void FinishTimer()
		{
			base.FinishTimer();
			ResetAiming();
		}

		public void InitPool()
		{
			MarkSpawnData.InitPool();
		}

		public virtual void ResetAiming()
		{
			this.ResetAiming(Actor);
			for (var i = 0; i < SpawnedObjects.Count; i++)
			{
				SpawnedObjects[i].Destroy();
			}
			SpawnedObjects.Clear();
			SpawnedAimingPrefab = null;
		}

		public void SetTarget(Vector3 position)
		{
			SpawnedObjects.ForEach(s => { if (s != null) { s.transform.position = position; } });
		}

		public void Spawn()
		{
			SpawnMarkIfNull();
			if (!FindTargetProperties.SearchCompleted)
			{
				CurrentEntityManager.AddComponentData(CurrentEntity, new AutoAimTargetData());
				return;
			}

			FindTargetProperties.SearchCompleted = false;
		}

		private void SpawnMarkIfNull()
		{
			if (SpawnedObjects.Count == 0)
			{
				SpawnedObjects = ActorSpawn.Spawn(MarkSpawnData, Actor, Actor);
				if (SpawnedObjects != null && SpawnedObjects.Count > 0)
				{
					SpawnedAimingPrefab = SpawnedObjects[0];
				}
			}
		}
	}
}