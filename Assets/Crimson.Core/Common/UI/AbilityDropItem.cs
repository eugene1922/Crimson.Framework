using Assets.Crimson.Core.Components;
using Crimson.Core.Common;
using Crimson.Core.Components;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Common.UI
{
	public class AbilityDropItem : MonoBehaviour, IActorAbility
	{
		public Vector3 Offset;
		public IActor Actor { get; set; }
		public List<InventoryItemData> DropItems { get; private set; }
		public Vector3 DropPosition => OwnerTransform.TransformPoint(Offset);
		public Quaternion DropRotation => Quaternion.LookRotation(DropPosition - OwnerTransform.position);
		public Entity Entity { get; private set; }
		public EntityManager Manager { get; private set; }

		private Transform OwnerTransform => Actor.Owner.GameObject.transform;

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Entity = entity;
			Actor = actor;

			Manager = World.DefaultGameObjectInjectionWorld.EntityManager;
			DropItems = new List<InventoryItemData>();
		}

		public void Drop(InventoryItemData data)
		{
			var ability = Actor.Owner.Abilities.FirstOrDefault(s => s is AbilityInventory);

			if (ability == null)
			{
				return;
			}
			var inventory = ability as AbilityInventory;
			inventory.Remove(data);

			DropItems.Add(data);
			if (!Manager.HasComponent<NeedDropItemTag>(Entity))
			{
				Manager.AddComponentData(Entity, new NeedDropItemTag());
			}
		}

		public void Execute()
		{
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