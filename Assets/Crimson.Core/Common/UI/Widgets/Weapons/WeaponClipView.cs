using Assets.Crimson.Core.Components.Weapons;
using TMPro;
using UnityEngine;

namespace Assets.Crimson.Core.Common.UI.Widgets.Weapons
{
	public class WeaponClipView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _label;
		[SerializeField] private string _format = "{0}/{1}";
		[SerializeField] private string _emptyChar = "?";
		private IWeapon _lastWeapon;
		private WeaponClip _clip;

		public string Text { get => _label.text; set => _label.text = value; }

		public void Set(IWeapon weapon)
		{
			if (weapon is IHasClip component)
			{
				if (_lastWeapon != null)
				{
					weapon.OnShot -= Refresh;
					weapon.OnReload -= Refresh;
				}
				weapon.OnShot += Refresh;
				weapon.OnReload += Refresh;
				_lastWeapon = weapon;
				_clip = component.ClipData;
				Refresh();
			}
			else
			{
				Refresh(_emptyChar, _emptyChar);
			}
		}

		private void Refresh()
		{
			Refresh(_clip.Current, _clip.Capacity);
		}

		private void Refresh(object current, object capacity)
		{
			var state = string.Format(_format, current, capacity);
			Text = state;
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