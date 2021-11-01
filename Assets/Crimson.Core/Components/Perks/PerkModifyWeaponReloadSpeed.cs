using System;
using System.Linq;
using Crimson.Core.Common;
using Crimson.Core.Utils;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.Perks
{
    public class PerkModifyWeaponReloadSpeed : MonoBehaviour, IActorAbility, IPerkAbility
    {
        public float weaponReloadModifier = 0.5f;
        public IActor Actor { get; set; }

        private AbilityWeapon _abilityWeapon;

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
        }

        public void Execute()
        {
            _abilityWeapon = (AbilityWeapon) Actor.Abilities.FirstOrDefault(ability => ability is AbilityWeapon);

            if (_abilityWeapon == null) return;

            _abilityWeapon.CooldownTime *= weaponReloadModifier;

            UpdateUI();
        }

        public void Apply(IActor target)
        {
            var copy = target.GameObject.CopyComponent(this) as PerkModifyWeaponReloadSpeed;
            
            if (copy == null)
            {
                Debug.LogError("[PERK MODIFY WEAPON RELOAD SPEED] Error copying perk to Actor!");
                return;
            }
            var e = target.ActorEntity;
            copy.AddComponentData(ref e,target);
            
            if (!Actor.Spawner.AppliedPerks.Contains(copy)) Actor.Spawner.AppliedPerks.Add(copy);
            
            copy.Execute();
        }

        public void Remove()
        {
            _abilityWeapon = (AbilityWeapon) Actor.Spawner.Abilities.FirstOrDefault(ability => ability is AbilityWeapon);

            if (_abilityWeapon == null || Math.Abs(weaponReloadModifier) < 0.001f) return;

            _abilityWeapon.CooldownTime /= weaponReloadModifier;

            UpdateUI();
            
            if (Actor.Spawner.AppliedPerks.Contains(this)) Actor.Spawner.AppliedPerks.Remove(this);
            Destroy(this);
        }

        private void UpdateUI()
        {
            if (_abilityWeapon == null) return;

            // var reloadTimeFieldInfo =
            //     typeof(AbilitySpawnProjectile).GetField(nameof(_abilitySpawnProjectile.projectileReloadTime));

            // if (reloadTimeFieldInfo == null) return;
            //
            // var reloadTimeFieldAttribute = reloadTimeFieldInfo.GetCustomAttribute<CastToUI>(false).FieldId;
            //
            // var player = GetComponent<AbilityActorPlayer>();
            //
            // foreach (var receiver in player.UIReceiverList)
            // {
            //     ((UIReceiver) receiver)?.UpdateUIElementsData(reloadTimeFieldAttribute,
            //         reloadTimeFieldInfo.GetValue(_abilitySpawnProjectile));
            // }
        }
    }
}