using System;
using Unity.Entities;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Crimson.Core.Common
{
	public struct NotifyButtonActionExecutedData : IComponentData
	{
		public int ButtonIndex;
	}

	public class OnScreenCustomButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
	{
		[InputControl(layout = "Button")]
		public string buttonControlPath;

		private bool _repeatedInvokingOnHold = true;

		protected override string controlPathInternal
		{
			get => buttonControlPath;
			set => buttonControlPath = value;
		}

		public void ForceButtonRelease()
		{
			if (_repeatedInvokingOnHold)
			{
				return;
			}

			SendValueToControl(0.0f);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			throw new NotImplementedException();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData == null)
			{
				throw new ArgumentNullException(nameof(eventData));
			}

			if (_repeatedInvokingOnHold)
			{
				SendValueToControl(1.0f);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData == null)
			{
				throw new ArgumentNullException(nameof(eventData));
			}

			SendValueToControl(_repeatedInvokingOnHold ? 0.0f : 1.0f);
		}

		public void SetupButton(bool repeatedInvokingOnHold)
		{
			_repeatedInvokingOnHold = repeatedInvokingOnHold;
		}
	}
}