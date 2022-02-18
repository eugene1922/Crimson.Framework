using Assets.Crimson.Core.Common.UI.Widgets;
using Assets.Crimson.Core.Components;
using Crimson.Core.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Crimson.Core.Common.UI
{
	[HideMonoScript]
	public class UIInventory : MonoBehaviour, IUIElement
	{
		[ValueDropdown(nameof(UIAssociatedIds))]
		public string AssociatedValueID = "";

		private UIReceiver _receiver;
		[SerializeField] private InventoryView _view;
		public string AssociatedID => AssociatedValueID;

		public void SetData(object input)
		{
			if (!(input is List<InventoryItemData> items))
			{
				return;
			}

			_view.Refresh(items);
		}

		private List<string> UIAssociatedIds()
		{
#if UNITY_EDITOR
			if (_receiver == null)
			{
				_receiver = GetComponentInParent<UIReceiver>();
			}
			List<string> result = null;
			if (_receiver != null)
			{
				result= _receiver.UIAssociatedIds;
			}
			return result;
#else
            return null;
#endif
		}
	}
}