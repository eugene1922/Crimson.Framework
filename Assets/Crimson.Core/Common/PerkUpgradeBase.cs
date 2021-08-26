using System.Collections.Generic;
using Crimson.Core.Components;
using Crimson.Core.Enums;
using Crimson.Core.Loading;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crimson.Core.Common
{
    [HideMonoScript]
    public class PerkUpgradeBase : MonoBehaviour, IPerkUpgrade
    {
        public string perkName;
        public Sprite perkImage;
        public GameObject perkPrefab;

        public string PerkName => perkName;

        public Sprite PerkImage => perkImage;

        public GameObject PerkPrefab => perkPrefab;

        public virtual void SpawnPerk(IActor target)
        {
            var spawn = target.GameObject.AddComponent<AbilityActorSpawn>();
            var perkData = new ActorSpawnerSettings
            {
                objectsToSpawn = new List<GameObject> {perkPrefab},
                SpawnPosition = SpawnPosition.UseSpawnerPosition,
                parentOfSpawns = TargetType.None,
                runSpawnActionsOnObjects = true,
                destroyAbilityAfterSpawn = true
            };
            spawn.SpawnData = perkData;
            var targetActorEntity = target.ActorEntity;
            spawn.AddComponentData(ref targetActorEntity,target);
            spawn.Execute();
        }
    }
}