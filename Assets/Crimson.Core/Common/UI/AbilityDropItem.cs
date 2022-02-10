using Assets.Crimson.Core.Components;
using Crimson.Core.Common;
using Crimson.Core.Components;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Common.UI
{
    public class AbilityDropItem : MonoBehaviour, IActorAbility
    {
        public IActor Actor { get; set; }
        public Entity Entity { get; private set; }
        public EntityManager Manager { get; private set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Entity = entity;
            Actor = actor;

            Manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Manager.AddBuffer<SpawnPrefabData>(entity);
        }

        public void Drop(InventoryItemData data)
        {
            var ability = Actor.Owner.Abilities.FirstOrDefault(s => s is AbilityInventory);

            if (ability == null)
            {
                return;
            }
            var inventory = ability as AbilityInventory;

            var buffer = Manager.GetBuffer<SpawnPrefabData>(Entity);

            var spawnPrefabData = new SpawnPrefabData()
            {
                ID = data.ID,
                Position = inventory.DropPoint,
                Rotation = Quaternion.LookRotation(inventory.DropPoint - inventory.transform.position)
            };
            buffer.Add(spawnPrefabData);
            inventory.Remove(data);
            Manager.AddComponentData(Entity, new SpawnBuffer());
        }

        public void Execute()
        {
        }
    }
}