using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Crimson.Core.Common
{
	[HideMonoScript]
	[RequireComponent(typeof(Image))]
	public partial class UIProgressBar : MonoBehaviour, IUIProgressBar
	{
		[ValueDropdown(nameof(UIAssociatedIds))]
		public string AssociatedValueID = "";

		[ShowIf("@ProgressBarChangeType == ChangeType.ChangeByValue")]
		[ValueDropdown(nameof(UIAssociatedIds))]
		public string MaxValueID = "";

		[EnumToggleButtons] public ChangeType ProgressBarChangeType;
		private object _cachedCurrentProgressBarValue;
		private object _cachedInputValue;
		private Image _progressBar;
		private UIReceiver _receiver;

		public string AssociatedID => AssociatedValueID;
		public float MaxValue { get; set; }
		public string MaxValueAssociatedID => MaxValueID;

		public void Awake()
		{
			_progressBar = GetComponent<Image>();
			if (_progressBar == null)
			{
				Debug.LogError("[UI Progress Bar] Image component is required!");
			}
		}

		public void SetData(object input)
		{
			if (!input.IsNumericType() || _progressBar == null)
			{
				return;
			}

			if (!input.IsNumericType())
			{
				return;
			}

			if (_cachedCurrentProgressBarValue != null &&
				Convert.ToDecimal(_cachedCurrentProgressBarValue) == Convert.ToDecimal(input))
			{
				return;
			}

			var inputValue = new object();

			switch (ProgressBarChangeType)
			{
				case ChangeType.ChangeByValue:
					_cachedInputValue = input;

					if (MaxValue <= 0)
					{
						return;
					}

					var convertedInput = (float)Convert.ToDecimal(input);
					inputValue = convertedInput / MaxValue;

					break;

				case ChangeType.ChangeByTimer:
					inputValue = (float)input;
					break;
			}

			_progressBar.fillAmount = (float)Convert.ToDecimal(inputValue);
			_cachedCurrentProgressBarValue = Convert.ToDecimal(inputValue);
		}

		public void SetMaxValue(object maxValue)
		{
			if (!maxValue.IsNumericType())
			{
				return;
			}

			MaxValue = (float)Convert.ToDecimal(maxValue);

			RefreshProgressBarView();

			if (_cachedInputValue == null)
			{
				return;
			}

			SetData(_cachedInputValue);
			_cachedInputValue = null;
		}

		private void RefreshProgressBarView()
		{
			if (_cachedInputValue == null)
			{
				return;
			}

			_progressBar.fillAmount = (float)Convert.ToDecimal(_cachedInputValue) / MaxValue;
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
				result = _receiver.UIAssociatedIds;
			}
			return result;
#else
            return null;
#endif
		}
	}
}