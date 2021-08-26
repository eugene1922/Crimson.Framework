using System.Collections.Generic;
using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Crimson.Core.Loading.ActorSpawners
{
    [HideMonoScript][DoNotAddToEntity]
    public class LevelActorSpawnFromSO : MonoBehaviour, IActorSpawner, IComponentName
    {
        public string ComponentName
        {
            get => componentName;
            set => componentName = value;
        }

        [Space] [SerializeField] public string componentName = "";
        [Space]
        
        public LevelActorSpawnerDataSO spawnDataFile;

        public List<GameObject> SpawnedObjects { get; private set; }
        
        public void Spawn()
        {
            Assert.IsNotNull(spawnDataFile);
            SpawnedObjects = ActorSpawn.Spawn(spawnDataFile.SpawnData);
        }

        public void RunSpawnActions()
        {
            _ = ActorSpawn.RunSpawnActions(SpawnedObjects);
        }
    }
}