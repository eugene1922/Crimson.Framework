using Crimson.Core.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Crimson.Core.Loading
{
    public class LevelBootstrap : MonoBehaviour, IGameModeBootstrap
    {
        public bool useDummyMeta;

        private readonly List<IActorSpawner> _actorSpawners = new List<IActorSpawner>();

        public void CollectSpawners(List<IActorSpawner> spawners)
        {
            spawners.AddRange(GetComponentsInChildren<IActorSpawner>());
        }

        public void CollectSpawners()
        {
            CollectSpawners(_actorSpawners);
        }

        public void RunSpawners(List<IActorSpawner> spawners)
        {
            foreach (var spawner in spawners)
            {
                spawner.Spawn();
            }
        }

        public void RunSpawners()
        {
            RunSpawners(_actorSpawners);
        }

        public void Start()
        {
            CollectSpawners();
            RunSpawners();
        }
    }
}