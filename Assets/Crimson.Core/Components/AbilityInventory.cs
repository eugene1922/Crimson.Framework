using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
    public class AbilityInventory : MonoBehaviour, IActorAbility
    {
        public List<ushort> Items = new List<ushort>();
        public Vector3 Offset;

        public IActor Actor { get; set; }
        public Vector3 DropPoint => transform.TransformPoint(Offset);
        public Entity Entity { get; private set; }
        public EntityManager Manager { get; private set; }

        public void Add(ushort item)
        {
            Items.Add(item);
        }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Actor = actor;
            Entity = entity;
            Manager.AddBuffer<SpawnPrefabData>(Entity);
        }

        public void Drop(ushort item)
        {
            var buffer = Manager.GetBuffer<SpawnPrefabData>(Entity);

            var spawnPrefabData = new SpawnPrefabData()
            {
                ID = item,
                Position = DropPoint,
                Rotation = Quaternion.LookRotation(DropPoint - transform.position)
            };
            buffer.Add(spawnPrefabData);
        }

        public void Execute()
        { }

        public void Remove(ushort item)
        {
            Items.Remove(item);
            Drop(item);
        }

        [Button]
        private void DropFirstItem()
        {
            if (Items.Count == 0)
            {
                return;
            }

            var firstItem = Items[0];

            var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            manager.AddComponentData(Actor.ActorEntity, new RemoveItemData() { ID = firstItem });
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Offset, 0.5f);
        }

#endif
    }
}