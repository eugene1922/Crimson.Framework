using Assets.Crimson.Core.Components;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Crimson.Core.Common.UI.Widgets
{
	public class InventoryView : MonoBehaviour, IActorAbility
	{
		public UnityEvent<InventoryItemData> OnDrop;

		public UnityEvent<InventoryItemData> OnUse;

		[SerializeField] private Button _dropButton;

		[SerializeField] private PoolSpawner<InventoryItemView> _pool;

		[ReadOnly, SerializeField] private InventoryItemView _selectedItem;

		[SerializeField] private Button _useButton;

		public IActor Actor { get; set; }

		public InventoryItemView SelectedItem
		{
			get => _selectedItem;
			set
			{
				var isOld = _selectedItem == value;
				if (isOld)
				{
					return;
				}
				_selectedItem = value;
				OnChangeSelected(value);
			}
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_pool.Init();
			_dropButton.interactable = false;
			_dropButton.onClick.AddListener(DropSelected);
			_useButton.interactable = false;
			_useButton.onClick.AddListener(UseSelected);
		}

		public void Execute()
		{
		}

		public void Refresh(List<InventoryItemData> items)
		{
			SelectedItem = null;
			_pool.ReleaseAll();
			for (var i = 0; i < items.Count; i++)
			{
				var view = CreateItem();
				view.SetData(items[i]);
				view.SetLabel(items[i].ID);
			}
		}

		private InventoryItemView CreateItem()
		{
			var instance = _pool.Get();
			instance.OnClick += SetSelectedView;
			return instance;
		}

		private void DropSelected()
		{
			if (SelectedItem == null)
			{
				return;
			}
			OnDrop?.Invoke(_selectedItem.Data);
		}

		private void OnChangeSelected(InventoryItemView value)
		{
			var isValid = value != null;

			_useButton.interactable = isValid;
			_dropButton.interactable = isValid;
		}

		private void SetSelectedView(InventoryItemView view)
		{
			SelectedItem = view;
		}

		private void UseSelected()
		{
			if (SelectedItem == null)
			{
				return;
			}
			OnUse?.Invoke(SelectedItem.Data);
		}
	}
}