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
		private WeaponClip _clip;

		public string Text { get => _label.text; set => _label.text = value; }

		public void Set(IHasClip weapon)
		{
			if (weapon == null)
			{
				Refresh(_emptyChar, _emptyChar);
			}
			else
			{
				if (_clip != null)
				{
					_clip.OnUpdate -= Refresh;
				}
				_clip = weapon.ClipData;
				_clip.OnUpdate += Refresh;
				Refresh();
			}
		}

		private void Refresh()
		{
			Refresh(_clip.Current, _clip.Count);
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