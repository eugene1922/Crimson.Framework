using Assets.Crimson.Core.Loading.SpawnDataTypes;
using Crimson.Core.Components;
using Crimson.Core.Enums;
using Crimson.Core.Loading.SpawnDataTypes;
using Crimson.Core.Utils;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Crimson.Core.Common
{
    public class ActorSpawn
    {
        [CanBeNull]
        public static List<SpawnItemData> GenerateData(IActorSpawnerSettings spawnSettings, IActor spawner = null,
            IActor owner = null)
        {
            if (spawnSettings.SpawnerDisabled)
            {
                return null;
            }

            int spawnCount;

            var sampledComponents = new List<Component>();

            if (spawnSettings.SpawnPosition == SpawnPosition.UseSpawnPoints &&
                spawnSettings.SpawnPointsFrom == SpawnPointsSource.FindByTag)
            {
                spawnSettings.SpawnPoints = GameObject.FindGameObjectsWithTag(spawnSettings.SpawnPointTag).ToList();
                if (spawnSettings.SpawnPoints.Count == 0)
                {
                    Debug.LogError("[LEVEL ACTOR SPAWNER] No spawn points found with tag: " +
                                   spawnSettings.SpawnPointTag + ". Aborting!");
                    return null;
                }
            }

            switch (spawnSettings.FillSpawnPoints)
            {
                case FillMode.UseEachObjectOnce:
                    spawnCount = spawnSettings.ObjectsToSpawn.Count;
                    break;

                case FillMode.FillAllSpawnPoints:
                    spawnCount = spawnSettings.SpawnPoints.Count;
                    break;

                case FillMode.PlaceEachObjectXTimes:
                    spawnCount = spawnSettings.ObjectsToSpawn.Count * spawnSettings.X;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (spawnSettings.SkipBusySpawnPoints && spawnSettings.SpawnPosition == SpawnPosition.UseSpawnPoints)
            {
                for (var i = 0; i < spawnSettings.SpawnPoints.Count; i++)
                {
                    var actorSpawnedOnPoint = spawnSettings.SpawnPoints[i].GetComponent<ActorSpawnedOnPoint>();

                    if (actorSpawnedOnPoint != null && actorSpawnedOnPoint.actor != null)
                    {
                        spawnSettings.SpawnPoints.RemoveAt(i);
                        spawnCount--;
                        i--;
                    }
                }
            }

            if (spawnSettings.SpawnPointsFillingMode == FillOrder.RandomOrder)
            {
                spawnSettings.ObjectsToSpawn =
                    spawnSettings.ObjectsToSpawn.OrderBy(item => spawnSettings.Rnd.Next()).ToList();
                spawnSettings.SpawnPoints =
                    spawnSettings.SpawnPoints.OrderBy(item => spawnSettings.Rnd.Next()).ToList();
            }

            var results = new List<SpawnItemData>(spawnCount);

            for (var i = 0; i < spawnCount; i++)
            {
                var data = new SpawnItemData
                {
                    Position = Vector3.zero,
                    Rotation = Quaternion.identity
                };
                switch (spawnSettings.SpawnPosition)
                {
                    case SpawnPosition.UseSpawnPoints:
                    {
                        if (spawnSettings.SpawnPoints.Count == 0)
                        {
                            throw new UnityException($"[ACTOR SPAWNER] In Use Spawn Points mode you have to provide some spawning points! \n" +
                                $"Spawner is {spawner}, and object is {spawnSettings.ObjectsToSpawn[0]}");
                        }
                        var point = spawnSettings.SpawnPoints[i % spawnSettings.SpawnPoints.Count];
                        data.Position = point.transform.position;
                        if (spawnSettings.RotationOfSpawns == RotationOfSpawns.UseSpawnPointRotation)
                        {
                            data.Rotation = point.transform.rotation;
                        }

                        break;
                    }

                    case SpawnPosition.RandomPositionOnNavMesh:
                        data.Position = NavMeshRandomPointUtil.GetRandomLocation();
                        break;

                    case SpawnPosition.UseSpawnerPosition:
                        if (spawner == null || spawner.GameObject == null)
                        {
                            throw new UnityException("[ACTOR SPAWNER] You are using Use Spawner Position, but Spawner is NULL!");
                        }

                        data.Position = spawner.GameObject.transform.position;
                        break;

                    case SpawnPosition.UseZeroPosition:
                        data.Position = Vector3.zero;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (spawnSettings.RotationOfSpawns)
                {
                    case RotationOfSpawns.UseRandomRotationY:
                        data.Rotation = Quaternion.Euler(0f, spawnSettings.Rnd.Next() % 360f, 0f);
                        break;

                    case RotationOfSpawns.UseRandomRotationXYZ:
                        data.Rotation = Quaternion.Euler(spawnSettings.Rnd.Next() % 360f, spawnSettings.Rnd.Next() % 360f,
                            spawnSettings.Rnd.Next() % 360f);
                        break;

                    case RotationOfSpawns.UseSpawnPointRotation:
                        if (spawnSettings.SpawnPosition == SpawnPosition.UseSpawnerPosition)
                        {
                            data.Rotation = spawner.GameObject.transform.rotation;
                        }

                        break;

                    case RotationOfSpawns.UseZeroRotation:
                        data.Rotation = Quaternion.Euler(Vector3.zero);
                        break;

                    case RotationOfSpawns.SpawnerMovement:
                        try
                        {
                            var spawnerMovementData =
                                World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ActorMovementData>(
                                    spawner.ActorEntity);
                            data.Rotation = spawnerMovementData.Input.Equals(float3.zero) ? Quaternion.Euler(Vector3.zero) : Quaternion.LookRotation(Vector3.Normalize(spawnerMovementData.Input));
                        }
                        catch
                        {
                            Debug.LogError(
                                "[ACTOR SPAWNER] To get Rotation from Spawner Movement, you need IActor Spawner and ActorMovementData on it!");
                            data.Rotation = Quaternion.Euler(Vector3.zero);
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (spawnSettings.CopyComponentsFromSamples.Count > 0)
                {
                    sampledComponents = new List<Component>();

                    foreach (var sample in spawnSettings.CopyComponentsFromSamples)
                    {
                        if (sample == null) continue;

                        foreach (var component in sample.GetComponents<Component>())
                        {
                            if (component == null || component is Transform || component is IActor) continue;
                            switch (spawnSettings.CopyComponentsOfType)
                            {
                                case ComponentsOfType.OnlyAbilities when !(component is IActorAbility):
                                    continue;
                                case ComponentsOfType.OnlySimpleBehaviours when component is IActorAbility:
                                    continue;
                                case ComponentsOfType.AllComponents:
                                default:
                                    sampledComponents.Add(component);
                                    break;
                            }
                        }
                    }

                    if (sampledComponents.Count == 0)
                        Debug.LogError("[LEVEL ACTOR SPAWNER] No suitable components found in sample game objects!");
                }

                data.Prefab = spawnSettings.ObjectsToSpawn[i % spawnSettings.ObjectsToSpawn.Count];
                data.SampledComponents = sampledComponents.ToArray();
                data.Spawner = spawner;
                data.Owner = owner;
                data.DeleteExistingComponents = spawnSettings.DeleteExistingComponents;

                if (spawnSettings.ParentOfSpawns != TargetType.None)
                {
                    var parentData = new SpawnParentData
                    {
                        Target = spawnSettings.ParentOfSpawns,
                        ComponentName = spawnSettings.ActorWithComponentName,
                        Tag = spawnSettings.ParentTag,
                        TargetStrategy = spawnSettings.ChooseStrategy
                    };

                    data.Parent = parentData;
                }

                results.Add(data);
            }

            return results;
        }

        private static List<IActor> RunSpawnActions(IEnumerable<GameObject> objects)
        {
            if (objects == null) return null;

            var objectList = objects.ToList();

            if (!objectList.Any()) return null;

            var actors = objectList.Select(o => o.GetComponent<IActor>()).Where(actorComponent => actorComponent != null)
                .ToList();

            foreach (var a in actors)
            {
                a.PerformSpawnActions();
            }

            return actors;
        }

        public static List<GameObject> Spawn(IActorSpawnerSettings spawnSettings, IActor spawner = null,
                            IActor owner = null)
        {
            var spawnItems = GenerateData(spawnSettings, spawner, owner);
            var results = new List<GameObject>();
            for (var i = 0; i < spawnItems.Count; i++)
            {
                results.Add(Spawn(spawnItems[i]));
            }
            return results;
        }

        public static GameObject Spawn(SpawnItemData data)
        {
            var result = Object.Instantiate(data.Prefab, data.Position, data.Rotation);

            var actors = result.GetComponents<IActor>();

            if (!data.Parent.IsEmpty)
            {
                var parents = FindActorsUtils.GetActorsList(result, data.Parent.Target,
                       data.Parent.ComponentName, data.Parent.Tag);

                var parent = FindActorsUtils.ChooseActor(result.transform, parents, data.Parent.TargetStrategy);

                result.transform.SetParent(parent);
            }

            if (data.SampledComponents.Length > 0)
            {
                if (data.DeleteExistingComponents)
                {
                    foreach (var component in result.GetComponents<Component>())
                    {
                        if (component is Transform || component is IActor)
                        {
                            continue;
                        }

                        Object.Destroy(component);
                    }
                }

                result.CopyComponentsWithLinks(data.SampledComponents);
            }

            if (actors.Length > 1)
            {
                Debug.LogError("[ACTOR SPAWNER] Only one IActor Component for Actor allowed!");
            }
            else if (actors.Length == 1)
            {
                var targetActor = actors[0];
                if (data.Spawner != null)
                {
                    data.Spawner.ChildrenSpawned++;
                    targetActor.ActorId = data.Spawner.ActorId;
                }

                targetActor.Spawner = data.Spawner;
                targetActor.Owner = data.Owner ?? data.Spawner ?? targetActor;
                targetActor.Setup();
                targetActor.PerformSpawnActions();
            }

            if (data.SpawnPoint != null)
            {
                data.SpawnPoint.AddComponent<ActorSpawnedOnPoint>().actor = result;
            }

            return result;
        }
    }
}