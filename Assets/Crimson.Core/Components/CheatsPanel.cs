using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crimson.Core.Components
{
    [HideMonoScript]
    public partial class CheatsPanel : MonoBehaviour, IDragHandler
    {
        public Button showCheatsButton;

        public Button backButton;
        public GameObject cheatsWindow;

        public GameObject cheatButtonTemplate;
        public GameObject content;

        [SerializeField] private RectTransform dragRectTransform;
        
        public void OnDrag(PointerEventData eventData)
        {
            dragRectTransform.anchoredPosition += eventData.delta;
        }

        private void Start()
        {
#if NO_CHEATS
            gameObject.SetActive(false);
            return;
#endif
            
            showCheatsButton.onClick.AddListener(ShowCheatWindow);
        }

        private void ShowCheatWindow()
        {
            if (!cheatsWindow.activeSelf)
            {
                cheatsWindow.SetActive(true);
                CreateButtons();
                return;
            }
            
            RemoveOldButtons();
            cheatsWindow.SetActive(false);
        }

        private void CreateButtons()
        {
            backButton.gameObject.SetActive(false);
            
            // Show Cheats Menu
            var button = Instantiate(cheatButtonTemplate, content.transform);
            var btn = button.GetComponent<CheatButton>();
            btn.ButtonAction = () =>
            {
                RemoveOldButtons();
                CreatePerksMenuButton();
            };
            
            btn.ButtonName = "Cheats";
        }

        private void RemoveOldButtons()
         {
             foreach (Transform child in content.transform)
             {
                 Destroy(child.gameObject);
             }
         }

         private void SetButtonAction(Action act)
         {
             backButton.onClick.RemoveAllListeners();
             
             backButton.onClick.AddListener(() =>
             {
                 RemoveOldButtons();
                 act.Invoke();
             });
         }
    }
}