using Crimson.Core.Common;
using Crimson.Core.Utils;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Crimson.Core.Common.UI
{
	[HideMonoScript]
	public class UITextNotification : MonoBehaviour, IUIElement
	{
		[ValueDropdown(nameof(UIAssociatedIds))] public string associatedFieldID = "";
		public string fieldText;
		[SerializeField] private AnimationCurve _showAnimation;
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private float _duration;
		private UIReceiver _receiver = null;
		private Sequence _sequence;
		private TMP_Text _textMeshPro;
		public string AssociatedID => associatedFieldID;

		public void Awake()
		{
			_textMeshPro = this.GetComponent<TMP_Text>();
			if (_textMeshPro == null)
			{
				Debug.LogError("[UI Text Field] TextMeshProGUI Component is required !");
			}
		}

		private void StartSequence()
		{
			if (_sequence == null)
			{
				_sequence = DOTween.Sequence();
			}
			_sequence.Pause();
			_sequence.Append(DOVirtual.Float(0, 1, _duration, ShowNotification));
			_sequence.Play();
		}

		public void SetData(object input)
		{
			if (_textMeshPro == null
				|| (!input.IsNumericType() && !(input is string)))
			{
				return;
			}

			StartSequence();
			_textMeshPro.SetText($"{fieldText}{input}");
		}

		private void ShowNotification(float alpha)
		{
			_canvasGroup.alpha = DOVirtual.EasedValue(0, 1, alpha, _showAnimation);
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