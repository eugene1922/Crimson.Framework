using Assets.Crimson.Core.Components;
using Crimson.Core.Common;
using Crimson.Core.Components;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Common.UI
{
    public class AbilityUseItem : MonoBehaviour, IActorAbility
    {
        private Entity _entity;

        public IActor Actor { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            _entity = entity;
            Actor = actor;
        }

        public void Execute()
        {
        }

        public void Use(InventoryItemData data)
        {
            var ability = Actor.Owner.Abilities.FirstOrDefault(s => s is AbilityInventory);

            if (ability == null)
            {
                return;
            }
            var inventory = ability as AbilityInventory;


            inventory.Remove(data);
        }
    }
}