using Crimson.Core.Common;
using Crimson.Core.Loading;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
    [HideMonoScript]
    [DoNotAddToEntity]
    public class AbilityActorSpawn : MonoBehaviour, IActorAbility, IActorSpawnerAbility, IComponentName
    {
        public IActor Actor { get; set; }

        public string ComponentName
        {
            get => componentName;
            set => componentName = value;
        }

        [Space]
        [SerializeField]
        public string componentName = "";

        [Space] public ActorSpawnerSettings SpawnData = new ActorSpawnerSettings();

        [Space] public bool ExecuteOnAwake = false;
        public List<GameObject> SpawnedObjects { get; private set; } = new List<GameObject>();
        public List<Action<GameObject>> SpawnCallbacks { get; set; } = new List<Action<GameObject>>();
        public Action<GameObject> DisposableSpawnCallback { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
        }

        void Start()
        {
            if (ExecuteOnAwake) Execute();
        }

        [ContextMenu("Execute")]
        public void Execute()
        {
            Spawn();
            RunSpawnActions();
            DestroyAbilityAfterSpawn();
        }

        public void Spawn()
        {
            SpawnedObjects = ActorSpawn.Spawn(SpawnData, Actor, Actor.Owner);
        }

        public void RunSpawnActions()
        {
            if (SpawnData.RunSpawnActionsOnObjects)
            {
                _ = ActorSpawn.RunSpawnActions(SpawnedObjects);
            }
        }

        private void DestroyAbilityAfterSpawn()
        {
            if (!SpawnData.DestroyAbilityAfterSpawn) return;

            Destroy(this);
        }
    }
}