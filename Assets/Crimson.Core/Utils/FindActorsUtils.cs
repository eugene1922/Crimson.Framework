using System;
using System.Collections.Generic;
using System.Linq;
using Crimson.Core.Common;
using Crimson.Core.Enums;
using Sirenix.Utilities;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Crimson.Core.Utils
{
    public static class FindActorsUtils
    {
        public static List<Transform> GetActorsList(GameObject source, IActor spawner, TargetType targetType, string name, string tag, bool childrenOnly = false)
        {
            var targets = new List<Transform>();
            var sourceTransform = source.transform;
            Transform spawnerTransform = null;
            switch (targetType)
            {
                case TargetType.ComponentName:
                    if (childrenOnly)
                    {
                        spawnerTransform = spawner?.GameObject.transform;
                    }
                    
                    Object.FindObjectsOfType<MonoBehaviour>().OfType<IComponentName>()
                        .Where(n => n.ComponentName.Equals(name,
                            StringComparison.Ordinal))
                        .ForEach(n =>
                        {
                            var c = (n as MonoBehaviour)?.gameObject.transform;
                            if (c == null) return;
                            if (childrenOnly && (spawnerTransform == null || !c.IsChildOf(spawnerTransform))) return;
                            
                            targets.Add(c);
                        });
                    break;
                case TargetType.ChooseByTag:
                    if (childrenOnly)
                    {
                        spawnerTransform = spawner?.GameObject.transform;
                    }
                    if(!tag.IsNullOrEmpty()) GameObject.FindGameObjectsWithTag(tag).ForEach(o =>
                    {
                        var c = o.transform;
                        if (childrenOnly && (spawnerTransform == null || !c.IsChildOf(spawnerTransform))) return;
                        targets.Add(o.transform);
                    });
                    break;
                case TargetType.Spawner:
                    var t = source.GetComponent<IActor>()?.Spawner;
                    if (t != null)
                    {
                        targets.Add(t.GameObject.transform);
                    }

                    break;
                case TargetType.None:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return targets;
        }

        public static Transform ChooseActor(Transform origin, IReadOnlyList<Transform> targets, ChooseTargetStrategy s)
        {
            Transform t;
            
            switch (targets.Count)
            {
                case 0: return null;
                case 1: return targets[0];
                default:
                    if (targets.Count == 0) return null;

                    t = targets[0];
                    float3 currentPosition = origin.position;
                    var currentDistance = math.distancesq(currentPosition, t.position);

                    if (s == ChooseTargetStrategy.FirstInChildren) return t;
                    if (s == ChooseTargetStrategy.Random) return targets[UnityEngine.Random.Range(0, targets.Count)];
                    
                    for (var i = 1; i < targets.Count; i++)
                    {
                        var tempDistanceSq = math.distancesq(currentPosition, targets[i].position);
                        
                        if ((s != ChooseTargetStrategy.Nearest || !(tempDistanceSq < currentDistance)) &&
                            (s != ChooseTargetStrategy.Farthest || !(tempDistanceSq > currentDistance))) continue;
                        
                        currentDistance = tempDistanceSq;
                        t = targets[i];
                    }

                    break;
            }

            return t;
        }
    }
}