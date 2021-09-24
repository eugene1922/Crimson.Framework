using Crimson.Core.Common;
using Crimson.Core.Enums;
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
    [DoNotAddToEntity]
    public class DragAim : TimerBaseBehaviour,
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
        [SerializeField] protected ActorSpawnerSettings _markSpawnData;

        [SerializeField, InfoBox("Time when aim disable (Seconds)")]
        private float _resetAimTime = 0.15f;

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

        public void EvaluateAim(Vector2 pos)
        {
            this.EvaluateAim(Actor as Actor, pos);
        }

        public void EvaluateAimBySelectedType(Vector2 pos)
        {
            EvaluateAimTarget(pos);
        }

        public void Execute()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator Here we need exact comparison
            if (Enabled && CurrentEntityManager.Exists(CurrentEntity))
            {
                Spawn();

                StartTimer();
                Timer.TimedActions.AddAction(FinishTimer, _resetAimTime);
            }
        }

        public override void FinishTimer()
        {
            base.FinishTimer();
            Enabled = true;
        }

        public void ResetSpawnPointRootRotation()
        {
            if (SpawnPointsRoot == null)
            {
                return;
            }
            SpawnPointsRoot.localRotation = Quaternion.identity;
        }

        public void Spawn()
        {
            if (SpawnedObjects == null || SpawnedObjects.Count == 0)
            {
                SpawnedObjects = ActorSpawn.Spawn(_markSpawnData);
            }
        }

        public override void StartTimer()
        {
            base.StartTimer();
            Enabled = false;
        }
        private bool _circlePrefabScaled;
        [SerializeField] private AimingProperties _aimingProperties;

        public AimingProperties AimingProperties { get => _aimingProperties; set => _aimingProperties = value; }

        public void EvaluateAimByArea(Vector2 pos)
        {
            var lastSpawnedAimingPrefabPos = AbilityUtils.EvaluateAimByArea(this, pos);

            if (_markSpawnData.SpawnPosition == SpawnPosition.UseSpawnPoints)
            {
                SpawnPointsRoot.rotation =
                    Quaternion.Euler(0, -180, 0) * SpawnedAimingPrefab.transform.rotation;
            }

            DisposableSpawnCallback = go =>
            {
                var targetActor = go.GetComponent<Actor>();
                if (targetActor == null) return;

                var vector = Quaternion.Euler(0, -180, 0) * lastSpawnedAimingPrefabPos;

                targetActor.ChangeActorForceMovementData(
                    _markSpawnData.SpawnPosition == SpawnPosition.UseSpawnPoints ? go.transform.forward : vector);
            };
        }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;

            CurrentEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            CurrentEntity = entity;

            SpawnCallbacks = new List<Action<GameObject>>();

            Enabled = true;

            CurrentEntityManager.AddComponent<TimerData>(entity);

            if (_markSpawnData.SpawnPoints != null
                && _markSpawnData.SpawnPoints.Count != 0)
            {
                _markSpawnData.SpawnPoints.Clear();
            }

            if (AimingAnimProperties.HasActorAimingAnimation)
            {
                CurrentEntityManager.AddComponentData(entity, new AimingAnimProperties
                {
                    AnimHash = Animator.StringToHash(AimingAnimProperties.ActorAimingAnimationName)
                });
            }
        }

        public void EvaluateAimByCircle()
        {
            if (_circlePrefabScaled)
            {
                return;
            }

            var objectsToSpawn = _markSpawnData.ObjectsToSpawn;
            if (objectsToSpawn.Count != 1)
            {
                return;
            }

            var objectToSpawn = objectsToSpawn[0];

            var abilityCollision = objectToSpawn.GetComponent<AbilityCollision>();

            var coll = abilityCollision?.useCollider;

            if (coll == null) return;

            var colliderRadius = 0f;

            switch (coll)
            {
                case SphereCollider sphere:
                    colliderRadius = sphere.radius;
                    break;

                case CapsuleCollider capsule:
                    var direction = capsule.direction;

                    colliderRadius = (direction == 0 || direction == 2)
                        ? capsule.height * 0.5f
                        : capsule.radius;
                    break;

                case BoxCollider box:
                    var size = box.size;
                    colliderRadius = Math.Max(size.x, size.z) * 0.5f;
                    break;
            }

            SpawnedAimingPrefab.transform.localScale *= colliderRadius;

            _circlePrefabScaled = true;
        }

        public void EvaluateAimBySight(Vector2 pos)
        {
            var lastSpawnedAimingPrefabPos = this.EvaluateAimBySight(Actor, pos);

            if (_markSpawnData.SpawnPosition == SpawnPosition.UseSpawnPoints)
            {
                SpawnPointsRoot.LookAt(SpawnedAimingPrefab.transform);
            }

            DisposableSpawnCallback = go =>
            {
                var targetActor = go.GetComponent<Actor>();
                if (targetActor == null)
                {
                    return;
                }

                var vector = lastSpawnedAimingPrefabPos - Actor.GameObject.transform.position;

                targetActor.ChangeActorForceMovementData(
                    _markSpawnData.SpawnPosition == SpawnPosition.UseSpawnPoints ? go.transform.forward : vector);

                CurrentEntityManager.AddComponentData(targetActor.ActorEntity, new DestroyProjectileInPointData
                {
                    Point = new float2(lastSpawnedAimingPrefabPos.x,
                        lastSpawnedAimingPrefabPos.z)
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

                case AimingType.Circle:
                    EvaluateAimByCircle();
                    break;
            }
        }

        public void ResetAiming()
        {
            this.ResetAiming(Actor);
            _circlePrefabScaled = false;
            if (AimingProperties.evaluateActionOptions != EvaluateActionOptions.RepeatingEvaluation)
            {
                return;
            }
            OnHoldAttackActive = false;
            ResetSpawnPointRootRotation();
        }
    }
}