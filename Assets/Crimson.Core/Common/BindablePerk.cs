using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Common
{
    [HideMonoScript]
    public class BindablePerk : PerkUpgradeBase
    {
        public override void SpawnPerk(IActor target)
        {
            var perk = perkPrefab.GetComponent<IPerkAbility>();

            var newComponents = (perk as IPerkAbilityBindable)?.CopyBindablePerk(target);

            if (newComponents == null) return;

            UpdatePerkButton(target, newComponents, out var updatedButtonReceiver);

            if (updatedButtonReceiver == null || !newComponents.Any()) return;

            foreach (var bindable in newComponents.OfType<IBindable>())
            {
                bindable.UpdateBindingIndex(updatedButtonReceiver.bindingIndex, target.ActorEntity);
            }
        }

        private void UpdatePerkButton(IActor target, List<Component> copiedComponents,
            out CustomButtonController updatedButtonController)
        {
            updatedButtonController = null;

            var targetPlayerAbility =
                target.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

            var addActionToInputAbility = copiedComponents.OfType<AbilityAddActionsToPlayerInput>().FirstOrDefault();

            if (addActionToInputAbility == null || targetPlayerAbility == null) return;

            var receivers = targetPlayerAbility.UIReceiverList;
            var customButtons = receivers.SelectMany(u => ((UIReceiver)u).customButtons);
            var buttonToUpdate = customButtons.FirstOrDefault(b => b.bindingIndex == addActionToInputAbility.customBinding.index);

            var stickControlAvailable = copiedComponents.OfType<IAimable>().FirstOrDefault()?.AimingAvailable;
            var repeatedInvokingOnHold = copiedComponents.OfType<AbilityWeapon>().FirstOrDefault()?.aimingProperties
                .evaluateActionOptions;

            if (buttonToUpdate != null)
            {
                buttonToUpdate.SetupCustomButton(perkName, perkImage, stickControlAvailable ?? false,
                    repeatedInvokingOnHold != null &&
                    repeatedInvokingOnHold == EvaluateActionOptions.RepeatingEvaluation);
            }

            updatedButtonController = buttonToUpdate;
        }
    }

    public struct ApplyPresetPerksData : IComponentData
    {
    }
}