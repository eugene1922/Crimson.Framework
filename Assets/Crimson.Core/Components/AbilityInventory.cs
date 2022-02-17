using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityInventory : MonoBehaviour, IActorAbility
	{
		[CastToUI(nameof(InventoryItems))]
		public List<InventoryItemData> InventoryItems = new List<InventoryItemData>();

		[HideInInspector] public List<IActor> UIReceiverList = new List<IActor>();
		private Dictionary<string, FieldInfo> _fieldsInfo = new Dictionary<string, FieldInfo>();
		public IActor Actor { get; set; }
		public Entity Entity { get; private set; }

		public void Add(InventoryItemData item)
		{
			InventoryItems.Add(item);
			UpdateUIData(nameof(InventoryItems));
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			InventoryItems.Clear();
			Actor = actor;
			Entity = entity;
			foreach (var fieldInfo in typeof(AbilityInventory).GetFields()
				.Where(field => field.GetCustomAttribute<CastToUI>(false) != null))
			{
				_fieldsInfo.Add(fieldInfo.Name, fieldInfo);
			}
		}

		public void Execute()
		{ }

		public void Remove(InventoryItemData item)
		{
			InventoryItems.Remove(item);
			UpdateUIData(nameof(InventoryItems));
		}

		private void UpdateUIData(string fieldName)
		{
			foreach (var receiver in UIReceiverList.Where(receiver => _fieldsInfo.ContainsKey(fieldName)))
			{
				((UIReceiver)receiver)?.UpdateUIElementsData(
					_fieldsInfo[fieldName].GetCustomAttribute<CastToUI>(false).FieldId,
					_fieldsInfo[fieldName].GetValue(this));
			}
		}
	}
}