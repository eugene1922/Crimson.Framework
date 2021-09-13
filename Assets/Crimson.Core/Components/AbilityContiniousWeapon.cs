using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Enums;
using Crimson.Core.Loading;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
    public class AbilityContiniousWeapon : TimerBaseBehaviour, IActorAbility, IActorSpawnerAbility, IComponentName, IEnableable,
        ICooldownable, IBindable
    {
        public IActor Actor { get; set; }

        public string ComponentName
        {
            get => componentName;
            set => componentName = value;
        }

        [Space]
        [ShowInInspector]
        [SerializeField]
        public string componentName = "";

        public bool primaryProjectile;

        public bool aimingAvailable;
        public bool deactivateAimingOnCooldown;

        [EnumToggleButtons] public AttackDirectionType attackDirectionType = AttackDirectionType.Forward;

        [Space] public ActorSpawnerSettings weaponSpawnItem;

        //TODO: Consider making this class child of AbilityActorSpawn, and leave all common fields to parent

        [Space] public float startupDelay = 0f;

        public float cooldownTime = 0.3f;

        [InfoBox("Clip Capacity of 0 stands for unlimited clip")]
        public int projectileClipCapacity = 0;

        [HideIf("projectileClipCapacity", 0f)] public float clipReloadTime = 1f;

        [InfoBox("Put here IEnable implementation to display reload")]
        [Space]
        public List<MonoBehaviour> reloadDisplayToggle = new List<MonoBehaviour>();

        [HideIf("projectileClipCapacity", 0f)]
        [Space]
        public List<MonoBehaviour> clipReloadDisplayToggle = new List<MonoBehaviour>();

        public ActorProjectileSpawnAnimProperties actorProjectileSpawnAnimProperties;

        public bool suppressWeaponSpawn = false;

        [HideInInspector] public List<string> appliedPerksNames = new List<string>();
        public List<GameObject> SpawnedObjects { get; private set; }
        public List<Action<GameObject>> SpawnCallbacks { get; set; }
        public Action<GameObject> DisposableSpawnCallback { get; set; }
        public bool Enabled { get; set; }

        public float CooldownTime
        {
            get => cooldownTime;
            set => cooldownTime = value;
        }

        public int BindingIndex { get; set; } = -1;

        public Transform SpawnPointsRoot { get; private set; }

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

        public bool ActionExecutionAllowed { get; set; }
        public GameObject SpawnedAimingPrefab { get; set; }

        private Entity _entity;
        private EntityManager _dstManager;
        private int _projectileClip;
        private bool _actorToUi;

        private bool _circlePrefabScaled;

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;

            _dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            _entity = entity;

            _projectileClip = projectileClipCapacity;

            SpawnCallbacks = new List<Action<GameObject>>();

            Enabled = true;

            _dstManager.AddComponent<TimerData>(entity);

            if (actorProjectileSpawnAnimProperties.HasActorProjectileAnimation)
            {
                _dstManager.AddComponentData(entity, new ActorProjectileAnimData
                {
                    AnimHash = Animator.StringToHash(actorProjectileSpawnAnimProperties.ActorProjectileAnimationName)
                });
            }

            appliedPerksNames = new List<string>();

            var playerActor = actor.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;
            _actorToUi = playerActor != null && playerActor.actorToUI;

            if (!Actor.Abilities.Contains(this)) Actor.Abilities.Add(this);

            if (!_actorToUi) return;

            SpawnPointsRoot = new GameObject("spawn points root").transform;
            SpawnPointsRoot.SetParent(gameObject.transform);

            SpawnPointsRoot.localPosition = Vector3.zero;
            ResetSpawnPointRootRotation();

            if (weaponSpawnItem.SpawnPosition == SpawnPosition.UseSpawnerPosition)
            {
                weaponSpawnItem.SpawnPosition = SpawnPosition.UseSpawnPoints;
            }

            if (weaponSpawnItem.SpawnPoints.Any()) weaponSpawnItem.SpawnPoints.Clear();

            var baseSpawnPoint = new GameObject("Base Spawn Point");
            baseSpawnPoint.transform.SetParent(SpawnPointsRoot);

            baseSpawnPoint.transform.localPosition = Vector3.zero;
            baseSpawnPoint.transform.localRotation = Quaternion.identity;

            weaponSpawnItem.SpawnPoints.Add(baseSpawnPoint);
        }

        public void Execute()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator Here we need exact comparison
            if (Enabled && startupDelay == 0 &&
                World.DefaultGameObjectInjectionWorld.EntityManager.Exists(_entity))
            {
                InstantExecute();
            }
            else if (Enabled && Timer != null)
            {
                Timer.TimedActions.AddAction(Spawn, startupDelay);
            }
        }

        private void InstantExecute()
        {
            Spawn();

            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(_entity,
                new ActorProjectileThrowAnimData());

            StartTimer();
            Timer.TimedActions.Clear();
            Timer.TimedActions.AddAction(FinishTimer, .15f);
        }

        public void ResetSpawnPointRootRotation()
        {
            if (attackDirectionType == AttackDirectionType.Forward)
            {
                SpawnPointsRoot.localRotation = Quaternion.identity;
                return;
            }

            SpawnPointsRoot.localRotation = Quaternion.Euler(0, -180, 0);
        }

        public void Spawn()
        {
            if (SpawnedObjects.Count != 0)
            {
                return;
            }

            SpawnedObjects = ActorSpawn.Spawn(weaponSpawnItem, Actor, Actor.Owner);

            var objectsToSpawn = suppressWeaponSpawn
                ? weaponSpawnItem.ObjectsToSpawn
                : SpawnedObjects;

            if (objectsToSpawn == null) return;

            foreach (var callback in SpawnCallbacks)
            {
                objectsToSpawn.ForEach(go => callback.Invoke(go));
            }

            objectsToSpawn.ForEach(go =>
            {
                DisposableSpawnCallback?.Invoke(go);
                DisposableSpawnCallback = null;
            });

            if (!_actorToUi) return;

            ResetSpawnPointRootRotation();
        }

        public void Reload()
        {
            _projectileClip = projectileClipCapacity;
        }

        public override void FinishTimer()
        {
            base.FinishTimer();
            for (var i = 0; i < SpawnedObjects.Count; i++)
            {
                Destroy(SpawnedObjects[i]);
            }
            SpawnedObjects.Clear();
        }

        public override void StartTimer()
        {
            base.StartTimer();
        }
    }
}
