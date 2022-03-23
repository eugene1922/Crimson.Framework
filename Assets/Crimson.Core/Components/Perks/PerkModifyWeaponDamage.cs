using System;
using System.Collections.Generic;
using System.Linq;
using Crimson.Core.Common;
using Crimson.Core.Enums;
using Crimson.Core.Loading;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.Perks
{
    [HideMonoScript]
    public class PerkModifyWeaponDamage : MonoBehaviour, IActorAbilityTarget, IPerkAbilityForSpawned
    {
        public bool ApplyToAllProjectiles = true;

        [HideIf("ApplyToAllProjectiles")] public string componentName = "";

        public float weaponDamageModifier  = 1.5f;
        public CollisionSettings collisionSettings;

        public  bool spawnTargetEffect;
        
        [ShowIf("spawnTargetEffect")]
        public GameObject targetEffectPrefab;
        
        public  bool spawnWeaponEffect;
        
        [ShowIf("spawnWeaponEffect")]
        public GameObject weaponEffectPrefab;

        public IActor Actor { get; set; }
        public IActor TargetActor { get; set; }
        public IActor AbilityOwnerActor { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
        }

        public void Execute()
        {
            if (spawnTargetEffect)
            {
                SpawnEffect(TargetActor, targetEffectPrefab);
            }
        }
        
        public void AddCollision(GameObject target)
        {
            var collision = target.GetComponent<AbilityCollision>();
            
            if (collision != null)
            {
                collision.collisionActions.Add(new CollisionAction
                {
                    collisionLayerMask = collisionSettings.collisionLayerMask,
                    useTagFilter = collisionSettings.useTagFilter,
                    filterMode = collisionSettings.filterMode,
                    filterTags = collisionSettings.filterTags,
                    executeOnCollisionWithSpawner = collisionSettings.executeOnCollisionWithSpawner,
                    destroyAfterAction = collisionSettings.destroyAfterAction,
                    actions = new List<MonoBehaviour>{this}
                });
            }
        }

        public void Apply(IActor target)
        {
            var copy = target.GameObject.CopyComponent(this) as PerkModifyWeaponDamage;
            
            if (copy == null)
            {
                Debug.LogError("[PERK MODIFY WEAPON DAMAGE] Error copying perk to Actor!");
                return;
            }
            var e = target.ActorEntity;
            copy.AddComponentData(ref e,target);

            if (!Actor.Spawner.AppliedPerks.Contains(copy)) Actor.Spawner.AppliedPerks.Add(copy);
            
            var projectiles = target.GameObject.GetComponents<AbilityWeapon>().ToList();
            
            if (!ApplyToAllProjectiles)
                projectiles = projectiles
                    .Where(p => p.ComponentName.Equals(componentName, StringComparison.Ordinal))
                    .ToList();
            
            foreach (var p in projectiles)
            {
                p.SpawnCallbacks.Add(copy.ApplyWeaponDamageModifier);
                p.SpawnCallbacks.Add(copy.AddCollisionAction);
                
                p.SpawnCallbacks.Add(go =>
                {
                    if (!spawnWeaponEffect) return;
                    
                    var targetActor = go.GetComponent<Actor>();
            
                    if (targetActor == null) return;
                    
                    SpawnEffect(targetActor, weaponEffectPrefab);
                });
            }
        }

        public void AddCollisionAction(GameObject target)
        {
            var p = target.CopyComponent(this) as PerkModifyWeaponDamage;
            
            if (p == null) return;
            
            var a = target.GetComponent<IActor>();
            if (a != null)
            {
                var e = a.ActorEntity;
                p.AddComponentData(ref e,a);
            }
            
            p.AddCollision(p.gameObject);
        }

        private void ApplyWeaponDamageModifier(GameObject target)
        {
            var targetActor = target.GetComponent<Actor>();
            
            if (targetActor == null) return;

            var applyDamageAbility = (AbilityApplyDamage) targetActor.Abilities.FirstOrDefault(ability => ability is AbilityApplyDamage);

            if (applyDamageAbility != null)
            {
                applyDamageAbility.Damage = (int) (applyDamageAbility.Damage * weaponDamageModifier);
            }
        }

        private void SpawnEffect(IActor target, GameObject prefab)
        {
            var effectData = new ActorSpawnerSettings
            {
                objectsToSpawn = new List<GameObject> {prefab},
                SpawnPosition = SpawnPosition.UseSpawnerPosition,
                parentOfSpawns = TargetType.None,
                runSpawnActionsOnObjects = true,
                destroyAbilityAfterSpawn = true
            };
            
            ActorSpawn.Spawn(effectData, target, Actor);
        }
        
        public void Remove()
        {
            if (Actor.Spawner.AppliedPerks.Contains(this)) Actor.Spawner.AppliedPerks.Remove(this);
            Destroy(this);
        }
    }
}