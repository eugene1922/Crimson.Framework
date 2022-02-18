using Assets.Crimson.Core.Components;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Crimson.Core.Common.UI.Widgets
{
	public class InventoryItemView : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private Image _icon;
		[SerializeField] private TMP_Text _label;

		public event Action<InventoryItemView> OnClick;

		public InventoryItemData Data { get; private set; }

		public void OnPointerClick(PointerEventData eventData)
		{
			OnClick?.Invoke(this);
		}

		public void SetData(InventoryItemData data)
		{
			Data = data;
		}

		public void SetIcon(Sprite sprite)
		{
			_icon.sprite = sprite;
		}

		public void SetLabel(object text)
		{
			_label.text = text.ToString();
		}

		private void OnDisable()
		{
			OnClick = null;
		}
	}
}