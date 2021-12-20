using System.Collections.Generic;
using UnityEngine;

namespace Crimson.Core.Common
{
    public interface IActorSpawner
    {
        List<GameObject> SpawnedObjects { get; }

        void InitPool();

        void Spawn();
    }
}