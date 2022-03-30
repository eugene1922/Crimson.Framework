using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityInventory : MonoBehaviour, IActorAbility
	{
		[CastToUI(nameof(InventoryItems))]
		public List<InventoryItemData> InventoryItems = new List<InventoryItemData>();

		[HideInInspector] public UIReceiverList UIReceiverList = new UIReceiverList();
		public IActor Actor { get; set; }
		public Entity Entity { get; private set; }

		public void Add(InventoryItemData item)
		{
			InventoryItems.Add(item);
			UIReceiverList.UpdateUIData(nameof(InventoryItems));
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			InventoryItems.Clear();
			Actor = actor;
			Entity = entity;
			UIReceiverList.Init(this, entity);
		}

		public void Execute()
		{ }

		public void Remove(InventoryItemData item)
		{
			InventoryItems.Remove(item);
			UIReceiverList.UpdateUIData(nameof(InventoryItems));
		}
	}
}