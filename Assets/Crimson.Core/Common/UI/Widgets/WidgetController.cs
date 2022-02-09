using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Crimson.Core.Common.UI.Widgets
{
    public class WidgetController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private bool _disableOnAwake;
        [SerializeField] private InputActionReference _visibilityAction;

        public bool IsVisible
        {
            get
            {
                return _canvasGroup.alpha == 1
                    && _canvasGroup.blocksRaycasts
                    && _canvasGroup.interactable;
            }

            set
            {
                _canvasGroup.alpha = value ? 1 : 0;
                _canvasGroup.interactable = value;
                _canvasGroup.blocksRaycasts = value;
            }
        }

        private void Awake()
        {
            IsVisible = !_disableOnAwake;
            _visibilityAction.action.performed += PerformedHandler;
            _visibilityAction.action.Enable();
        }

        private void PerformedHandler(InputAction.CallbackContext context)
        {
            Toggle();
        }

        private void Toggle()
        {
            IsVisible = !IsVisible;
        }
    }
}