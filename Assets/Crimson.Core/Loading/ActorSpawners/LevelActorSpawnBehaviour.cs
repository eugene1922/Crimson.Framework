using Crimson.Core.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Crimson.Core.Loading.ActorSpawners
{
    [HideMonoScript]
    public sealed class LevelActorSpawnBehaviour : MonoBehaviour, IActorSpawner, IComponentName
    {
        [Space] [SerializeField] public string componentName = "";

        [Space]
        public ActorSpawnerSettings SpawnData;

        public string ComponentName
        {
            get => componentName;
            set => componentName = value;
        }

        public List<GameObject> SpawnedObjects { get; private set; } = new List<GameObject>();

        public void InitPool()
        {
            SpawnData.InitPool();
        }

        public void Spawn()
        {
            var spawnItems = ActorSpawn.GenerateData(SpawnData);
            SpawnedObjects = new List<GameObject>();
            for (var i = 0; i < spawnItems.Count; i++)
            {
                SpawnedObjects.Add(ActorSpawn.Spawn(spawnItems[i]));
            }
        }
    }
}