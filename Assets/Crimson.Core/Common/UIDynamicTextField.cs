using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Crimson.Core.Common
{
	[HideMonoScript]
	public class UIDynamicTextField : MonoBehaviour, IUIElement
	{
		[ValueDropdown(nameof(UIAssociatedIds))] public string associatedFieldID = "";
		public string fieldText;
		private object _cachedFieldValue = new object();
		private UIReceiver _receiver = null;
		private TMP_Text _textMeshPro;
		public string AssociatedID => associatedFieldID;

		public void Awake()
		{
			_textMeshPro = this.GetComponent<TMP_Text>();
			if (_textMeshPro == null)
				Debug.LogError("[UI Text Field] TextMeshProGUI Component is required !");
		}

		public void SetData(object input)
		{
			if (_textMeshPro == null) return;

			if (!input.IsNumericType() && !(input is string)) return;

			if (_cachedFieldValue.Equals(input)) return;

			_textMeshPro.SetText($"{fieldText}{input}");
			_cachedFieldValue = input;
		}

		private List<string> UIAssociatedIds()
		{
#if UNITY_EDITOR
			if (_receiver == null) _receiver = GetComponentInParent<UIReceiver>();
			return _receiver == null ? null : _receiver.UIAssociatedIds;
#else
            return null;
#endif
		}
	}
}