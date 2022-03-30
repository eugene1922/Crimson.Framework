using Assets.Crimson.Core.Components.Weapons;
using Crimson.Core.Common;
using TMPro;
using UnityEngine;

namespace Assets.Crimson.Core.Common.UI.Widgets.Weapons
{
	public class WeaponNameLabel : MonoBehaviour
	{
		[SerializeField] private TMP_Text _label;
		[SerializeField] private string _emptyName = "No name";

		public string Text { get => _label.text; set => _label.text = value; }

		public void Set(IWeapon weapon)
		{
			var name = _emptyName;
			if (weapon is IHasComponentName componentName && !string.IsNullOrEmpty(componentName.ComponentName))
			{
				name = componentName.ComponentName;
			}

			_label.text = name;
		}

#if UNITY_EDITOR

		private void OnValidate()
		{
			if (_label == null)
			{
				_label = GetComponent<TMP_Text>();
			}
		}

#endif
	}
}