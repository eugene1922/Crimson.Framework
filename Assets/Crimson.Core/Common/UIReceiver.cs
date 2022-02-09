using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Common
{
    public struct UIReceiverData : IComponentData
    {
    }

    [HideMonoScript]
    public class UIReceiver : Actor
    {
        public List<CustomButtonController> customButtons = new List<CustomButtonController>();

        [Space]
        [OnValueChanged("UpdateUIChannelInfo")]
        public bool explicitUIChannel;

        [InfoBox("If nothing is specified here, will use UIFieldElement instances found in children automatically")]
        [ValidateInput("MustBeUI", "UI MonoBehaviours must derive from IUIFieldElement!")]
        public List<MonoBehaviour> UIBehaviours = new List<MonoBehaviour>();
        [ShowIf("explicitUIChannel")] public int UIChannelID = 0;

        private static List<string> _ids;

        private List<string> _associatedIdsCache = new List<string>();

        private List<IUIElement> _uiElements = new List<IUIElement>();

        public List<string> UIAssociatedIds
        {
            get
            {
                if (_associatedIdsCache.Any()) return _associatedIdsCache;

                _associatedIdsCache = GetUIFieldsIDs();
                _associatedIdsCache.Insert(0, "No ID");

                return _associatedIdsCache;
            }
        }

        public List<IUIElement> UIElements
        {
            get
            {
                if (_uiElements.Count == 0)
                {
                    _uiElements = UIBehaviours.ConvertAll(b => b as IUIElement);
                }

                return _uiElements;
            }
        }
        public void NotifyButtonActionExecuted(int index)
        {
            foreach (var button in customButtons.Where(button => button.bindingIndex == index))
            {
                button.onScreenButtonComponent.ForceButtonRelease();
            }
        }

        public override void PostConvert()
        {
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<UIReceiverData>(ActorEntity);

            if (UIElements.Count != 0) return;

            _uiElements = GetComponentsInChildren<IUIElement>().ToList();
            for (var i = 0; i < _uiElements.Count; i++)
            {
                var element = _uiElements[i] as MonoBehaviour;
            }
            UIBehaviours = UIElements.ConvertAll(f => f as MonoBehaviour);

            foreach (var actor in GetComponentsInChildren<IActor>())
            {
                if (actor.Spawner != null) continue;
                actor.Spawner = Spawner;
            }

            if (_associatedIdsCache.Any()) return;

            _associatedIdsCache = GetUIFieldsIDs();
            _associatedIdsCache.Insert(0, "No ID");

            SetupAttackButton();
        }

        public void SetCustomButtonOnCooldown(int index, bool onCooldown)
        {
            foreach (var button in customButtons.Where(button => button.bindingIndex == index))
            {
                button.SetButtonOnCooldown(onCooldown);
            }
        }

        public void UpdateUIElementsData(string elementID, object updatedData)
        {
            var maxValuesToUpdate = UIElements.Where(element =>
                element is IUIProgressBar progressBar && progressBar.MaxValueAssociatedID.Equals(elementID)).ToList();
            maxValuesToUpdate.ForEach(progressBar => ((IUIProgressBar)progressBar).SetMaxValue(updatedData));

            var elementToUpdate = UIElements.Where(element => element.AssociatedID.Equals(elementID)).ToList();
            elementToUpdate.ForEach(element => element.SetData(updatedData));
        }
        private List<string> GetUIFieldsIDs()
        {
            var fields = Assembly.GetExecutingAssembly()
                .GetTypes().SelectMany(t => t.GetFields());

            if (_ids == null)
            {
                _ids = new List<string>();

                foreach (var field in fields)
                {
                    var attrs = field.GetCustomAttributes(false);

                    foreach (var attr in attrs)
                    {
                        if (attr is CastToUI castToUi)
                        {
                            _ids.Add(castToUi.FieldId);
                        }
                    }
                }
            }

            return _ids;
        }

        private bool MustBeUI(List<MonoBehaviour> f)
        {
            return !f.Exists(t => !(t is IUIElement)) || f.Count == 0;
        }

        private void SetupAttackButton()
        {
            if (Spawner == null) return;

            var abilityPlayerInput =
                Spawner.Abilities.FirstOrDefault(a => a is AbilityPlayerInput) as AbilityPlayerInput;

            if (abilityPlayerInput == null) return;

            AbilityWeapon primaryWeaponAbility = null;

            var primaryWeaponBinding = abilityPlayerInput.customBindings.FirstOrDefault(c =>
            {
                var primaryWeapon =
                    c.actions.FirstOrDefault(a => a is AbilityWeapon weapon && weapon.primaryProjectile);

                if (primaryWeapon != null)
                {
                    primaryWeaponAbility = primaryWeapon as AbilityWeapon;
                }

                return primaryWeapon != null;
            });

            if (primaryWeaponBinding.Equals(default(CustomBinding)) || primaryWeaponAbility == null) return;

            var primaryWeaponButton = customButtons.FirstOrDefault(b => b.bindingIndex == primaryWeaponBinding.index);

            if (primaryWeaponButton == null) return;

            primaryWeaponAbility.UpdateBindingIndex(primaryWeaponBinding.index, Spawner.ActorEntity);

            primaryWeaponButton.SetupCustomButton(((IAimable)primaryWeaponAbility).AimingAvailable,
                primaryWeaponAbility.aimingProperties.evaluateActionOptions ==
                EvaluateActionOptions.RepeatingEvaluation);
        }
        private void UpdateUIChannelInfo()
        {
#if UNITY_EDITOR
            if (!explicitUIChannel) UIChannelID = 0;
#endif
        }
    }
}