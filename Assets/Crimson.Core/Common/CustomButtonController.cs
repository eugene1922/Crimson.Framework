using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

namespace Crimson.Core.Common
{
	[HideMonoScript]
	public class CustomButtonController : TimerBaseBehaviour
	{
		public int bindingIndex;
		public Image buttonImage;
		public Image cooldownProgressBar;
		public bool reverseProgressBarDirection = false;
		public GameObject cooldownTimerRoot;
		public TextMeshProUGUI cooldownTimerText;

		public OnScreenCustomButton onScreenButtonComponent;

		public void SetupCustomButton(string perkName, Sprite perkSprite, bool stickControlAvailable, bool repeatedInvokingOnHold)
		{
			buttonImage.sprite = perkSprite;
			SetupCustomButton(stickControlAvailable, repeatedInvokingOnHold);
		}

		public void SetupCustomButton(bool stickControlAvailable, bool repeatedInvokingOnHold)
		{
			//onScreenButtonComponent.SetupButton(repeatedInvokingOnHold);
		}

		public void SetButtonOnCooldown(bool onCooldown)
		{
		}

		public void SetCooldownProgressBar(float value)
		{
			cooldownProgressBar.fillAmount = value;
		}

		public void SetCooldownText(int value)
		{
			if (cooldownTimerRoot == null)
			{
				return;
			}

			cooldownTimerRoot.SetActive(true);
			if (cooldownTimerText != null)
			{
				cooldownTimerText.text = value.ToString();
			}
		}

		public void HideCoolDownText()
		{
			if (cooldownTimerRoot == null)
			{
				return;
			}

			cooldownTimerRoot.SetActive(false);
		}
	}
}