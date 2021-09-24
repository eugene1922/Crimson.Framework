using Crimson.Core.Common;
using Crimson.Core.Loading;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.AbilityReactive
{
    [DoNotAddToEntity]
    public class AutoAim : TimerBaseBehaviour,
        IActorAbility,
        IActorSpawnerAbility,
        IComponentName,
        IEnableable,
        IAimable
    {
        public AimingAnimationProperties aimingAnimProperties;
        public bool aimingAvailable;

        [Space]
        [ShowInInspector]
        [SerializeField]
        public string componentName = "";

        public bool deactivateAimingOnCooldown;
        public ActorSpawnerSettings MarkSpawnData;

        [SerializeField] public FindTargetProperties FindTargetProperties = new FindTargetProperties();

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
        public bool Enabled { get; set; }
        public bool OnHoldAttackActive { get; set; }
        public List<Action<GameObject>> SpawnCallbacks { get; set; }
        public GameObject SpawnedAimingPrefab { get; set; }
        public List<GameObject> SpawnedObjects { get; private set; } = new List<GameObject>();
        public Transform SpawnPointsRoot { get; private set; }
        protected Entity CurrentEntity { get; private set; }
        protected EntityManager CurrentEntityManager { get; private set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;

            CurrentEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            CurrentEntity = entity;

            SpawnCallbacks = new List<Action<GameObject>>();

            Enabled = true;

            CurrentEntityManager.AddComponent<TimerData>(entity);

            if (MarkSpawnData.SpawnPoints != null
                && MarkSpawnData.SpawnPoints.Count != 0)
            {
                MarkSpawnData.SpawnPoints.Clear();
            }
        }

        public void EvaluateAim(Vector2 pos)
        {
            this.EvaluateAim(Actor as Actor, pos);
        }

        public void EvaluateAimBySelectedType(Vector2 pos)
        {
            EvaluateAimTarget(pos);
        }

        public void EvaluateAimTarget(Vector2 input)
        {
        }

        public void Execute()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator Here we need exact comparison
            if (Enabled && CurrentEntityManager.Exists(CurrentEntity))
            {
                Spawn();

                StartTimer();
                Timer.TimedActions.AddAction(FinishTimer, ResetAimTime);
            }
        }

        public override void FinishTimer()
        {
            base.FinishTimer();
            Enabled = true;
        }

        public virtual void ResetAiming()
        {
            this.ResetAiming(Actor);
            OnHoldAttackActive = false;
            ResetSpawnPointRootRotation();
        }

        public void ResetSpawnPointRootRotation()
        {
            if (SpawnPointsRoot == null)
            {
                return;
            }
            SpawnPointsRoot.localRotation = Quaternion.identity;
        }

        public void SetTarget(Vector3 position)
        {
            SpawnedObjects.ForEach(s => s.transform.position = position);
        }

        public void Spawn()
        {
            if (SpawnedObjects == null || SpawnedObjects.Count == 0)
            {
                SpawnedObjects = ActorSpawn.Spawn(MarkSpawnData, Actor, Actor);
            }
            if (!FindTargetProperties.SearchCompleted)
            {
                CurrentEntityManager.AddComponentData(CurrentEntity, new AutoAimTargetData());

                return;
            }

            FindTargetProperties.SearchCompleted = false;
        }

        public override void StartTimer()
        {
            base.StartTimer();
            Enabled = false;
        }
    }
}