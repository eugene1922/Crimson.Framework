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

        public OnScreenStick onScreenStickComponent;
        public OnScreenCustomButton onScreenButtonComponent;

        public void SetupCustomButton(string perkName, Sprite perkSprite, bool stickControlAvailable, bool repeatedInvokingOnHold)
        {
            buttonImage.sprite = perkSprite;
            SetupCustomButton(stickControlAvailable, repeatedInvokingOnHold);
        }

        public void SetupCustomButton(bool stickControlAvailable, bool repeatedInvokingOnHold)
        {
            SetStickState(stickControlAvailable);

            if (onScreenButtonComponent != null)
            {
                onScreenButtonComponent.SetupButton(repeatedInvokingOnHold);
            }
        }

        public void SetButtonOnCooldown(bool onCooldown)
        {
            SetStickState(!onCooldown);
        }

        private void SetStickState(bool state)
		{
            if (onScreenStickComponent != null)
            {
                onScreenStickComponent.enabled = state;
            }
        }

        public void SetCooldownProgressBar(float value)
        {
            cooldownProgressBar.fillAmount = value;
        }

        public void SetCooldownText(int value)
        {
            if (cooldownTimerRoot == null) return;
            cooldownTimerRoot.SetActive(true);
            if (cooldownTimerText != null) cooldownTimerText.text = value.ToString();
        }

        public void HideCoolDownText()
        {
            if (cooldownTimerRoot == null) return;
            cooldownTimerRoot.SetActive(false);
        }
    }
}