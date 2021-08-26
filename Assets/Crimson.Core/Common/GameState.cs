using System.Collections.Generic;
using Crimson.Core.Components;
using Crimson.Core.Enums;
using Crimson.Core.Loading;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Common
{
    public class GameState : Actor
    {
        public GameObject respawnPanel;
        public GameObject winPanel;
        public GameObject losePanel;

        public GameObject rootCanvas; 
        
        public int maxDeathCount = 1;

        [ValidateInput("MustBeSpawner", "Spawner MonoBehaviours must derive from IActorSpawner!!")]
        public MonoBehaviour userSpawner;

        [HideInInspector]
        public AbilityActorPlayer userPlayer;
        [HideInInspector]
        public List<AbilityActorPlayer> players;

        [HideInInspector] 
        public IYandexAppMetrica metrica;

        [HideInInspector] 
        public double startTime;

        [HideInInspector]
        public ActorSpawnerSettings sampleSpawner = new ActorSpawnerSettings
        {
            ObjectsToSpawn = null,
            SpawnPosition = SpawnPosition.UseSpawnerPosition,
            SpawnPointsFillingMode = FillOrder.SequentialOrder,
            FillSpawnPoints = FillMode.UseEachObjectOnce,
            SkipBusySpawnPoints = false,
            SpawnPoints = new List<GameObject>(),
            RotationOfSpawns = RotationOfSpawns.UseZeroRotation,
            ParentOfSpawns = TargetType.None,
            chooseParentStrategy = ChooseTargetStrategy.Nearest,
            RunSpawnActionsOnObjects = true,
            DestroyAbilityAfterSpawn = false,
            CopyComponentsFromSamples = new List<GameObject>( ),
            CopyComponentsOfType = ComponentsOfType.AllComponents
        };
        
        public override void PostConvert()
        {
            WorldEntityManager.AddComponentData(ActorEntity, new GameStateData());
        }

        protected override void Start()
        {
            sampleSpawner.SpawnPoints = new List<GameObject> {this.gameObject};
            sampleSpawner.objectsToSpawn = new List<GameObject>
            {
                respawnPanel,
                winPanel,
                losePanel
            };
            metrica = AppMetrica.Instance;
            Setup();
        }
        
        private bool MustBeSpawner(MonoBehaviour a)
        {
            return (a is IActorSpawner) || (a is null);
        }
    }

    public struct GameStateData : IComponentData
    {
        public byte foo;
    }
}