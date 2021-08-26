using System;
using System.Collections.Generic;
using UnityEngine;

namespace Crimson.Core.Common
{
    public interface IActorSpawnerAbility : IActorSpawner
    {
        List<Action<GameObject>> SpawnCallbacks { get; set; }
        Action<GameObject> DisposableSpawnCallback { get; set; }
    }
}