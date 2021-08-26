using System.Collections.Generic;
using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crimson.Core.Loading.ActorSpawners
{
    [HideMonoScript][DoNotAddToEntity]
    public sealed class LevelActorSpawnBehaviour : MonoBehaviour, IActorSpawner, IComponentName
    {
        public string ComponentName
        {
            get => componentName;
            set => componentName = value;
        }

        [Space] [SerializeField] public string componentName = "";
        [Space] 
        
        public ActorSpawnerSettings SpawnData;

        public List<GameObject> SpawnedObjects { get; private set; }
        
        public void Spawn()
        {
            SpawnedObjects = ActorSpawn.Spawn(SpawnData);
        }

        public void RunSpawnActions()
        {
            if (SpawnData.RunSpawnActionsOnObjects)
            {
                _ = ActorSpawn.RunSpawnActions(SpawnedObjects);
            }
        }
    }
}