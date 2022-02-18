using Crimson.Core.Common;
using Crimson.Core.Enums;
using Crimson.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crimson.Core.Components
{
	public partial class CheatsPanel
	{
		private static List<GameObject> _perksList = new List<GameObject>();

		private static IActor _playerToUi;

		public static List<GameObject> PerksList
		{
			get
			{
				if (_perksList.Count != 0)
				{
					return _perksList;
				}

				var availablePerksObject = FindObjectOfType(typeof(AvailablePerks)) as AvailablePerks;
				if (availablePerksObject != null)
				{
					_perksList = availablePerksObject.CheatPerksList;
				}

				return _perksList;
			}
		}

		public static IActor PlayerToUi
		{
			get
			{
				if (_playerToUi is object && _playerToUi.GameObject != null)
				{
					return _playerToUi;
				}

				var playersPrefabs = FindObjectsOfType<AbilityPlayerInput>();
				var input = playersPrefabs.FirstOrDefault(p => p != null && p.inputSource == InputSource.UserInput);
				if (input != null)
				{
					_playerToUi = input.Actor;
				}

				return _playerToUi;
			}
		}

		private void ChangeBindablePerk(int bindingIndex, BindablePerk perk)
		{
			if (PlayerToUi is null)
			{
				return;
			}

			var perkToApply = perk.perkPrefab.GetComponent<IPerkAbilityBindable>();
			var newComponents = perkToApply?.CopyBindablePerk(PlayerToUi, bindingIndex);

			if (newComponents == null)
			{
				return;
			}

			var targetPlayerAbility = PlayerToUi.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

			if (targetPlayerAbility == null)
			{
				return;
			}

			var buttonToUpdate = targetPlayerAbility.UIReceiverList
				 .SelectMany(u => ((UIReceiver)u).customButtons)
				 .FirstOrDefault(b => b.bindingIndex == bindingIndex);

			var stickControlAvailable = newComponents.OfType<IAimable>().FirstOrDefault()?.AimingAvailable;
			var weapon = newComponents.OfType<AbilityWeapon>().FirstOrDefault();
			EvaluateActionOptions? repeatedInvokingOnHold = null;
			if (weapon != null)
			{
				repeatedInvokingOnHold = weapon.aimingProperties.evaluateActionOptions;
			}

			if (buttonToUpdate != null)
			{
				buttonToUpdate.SetupCustomButton(perk.perkName, perk.perkImage, stickControlAvailable ?? false,
					repeatedInvokingOnHold != null && repeatedInvokingOnHold == EvaluateActionOptions.RepeatingEvaluation);
			}

			if (buttonToUpdate is null || !newComponents.Any())
			{
				return;
			}

			foreach (var bindable in newComponents.OfType<IBindable>())
			{
				bindable.UpdateBindingIndex(buttonToUpdate.bindingIndex, PlayerToUi.ActorEntity);
			}

			if (stickControlAvailable == null || (bool)!stickControlAvailable || repeatedInvokingOnHold == null)
			{
				return;
			}

			var weaponAbilities = newComponents.OfType<AbilityWeapon>().ToList();

			foreach (var ability in weaponAbilities)
			{
				ability.aimingProperties.evaluateActionOptions = (EvaluateActionOptions)repeatedInvokingOnHold;
			}
		}

		private void CreateChooseBindablePerksButton()
		{
			SetButtonAction(CreatePerksMenuButton);

			var button = Instantiate(cheatButtonTemplate, content.transform);
			var btn = button.GetComponent<CheatButton>();
			btn.ButtonAction = () =>
			{
				RemoveOldButtons();
				ShowAvailableBindablePerks(2);
			};
			btn.ButtonName = "Choose first perk";

			button = Instantiate(cheatButtonTemplate, content.transform);
			btn = button.GetComponent<CheatButton>();
			btn.ButtonAction = () =>
			{
				RemoveOldButtons();
				ShowAvailableBindablePerks(3);
			};
			btn.ButtonName = "Choose second perk";

			button = Instantiate(cheatButtonTemplate, content.transform);
			btn = button.GetComponent<CheatButton>();
			btn.ButtonAction = () =>
			{
				RemoveOldButtons();
				ShowAvailableBindablePerks(4);
			};
			btn.ButtonName = "Choose third perk";
		}

		private void CreatePerksMenuButton()
		{
			backButton.gameObject.SetActive(true);
			SetButtonAction(CreateButtons);

			var button = Instantiate(cheatButtonTemplate, content.transform);
			var btn = button.GetComponent<CheatButton>();
			btn.ButtonAction = () =>
			{
				RemoveOldButtons();
				CreateChooseBindablePerksButton();
			};

			btn.ButtonName = "Bindable Perks";

			button = Instantiate(cheatButtonTemplate, content.transform);
			btn = button.GetComponent<CheatButton>();
			btn.ButtonAction = () =>
			{
				RemoveOldButtons();
				ShowAndApplyBasePerk();
			};

			btn.ButtonName = "Base Perks";
		}

		private void ShowAndApplyBasePerk()
		{
			var availablePerks = PerksList
				.Where(p =>
				{
					var perk = p.GetComponent<PerkUpgradeBase>();

					return perk != null && !perk.GetType().IsSubclassOf(typeof(PerkUpgradeBase));
				}).ToList();

			foreach (var go in availablePerks)
			{
				var perk = go.GetComponent<PerkUpgradeBase>();

				var button = Instantiate(cheatButtonTemplate, content.transform);
				var btn = button.GetComponent<CheatButton>();
				btn.ButtonAction = () =>
				{
					if (PlayerToUi is null)
					{
						return;
					}

					perk.SpawnPerk(_playerToUi);
				};
				btn.ButtonName = perk.PerkName;
			}
		}

		private void ShowAvailableBindablePerks(int perkIndexToChange)
		{
			SetButtonAction(CreateChooseBindablePerksButton);

			var availablePerks = PerksList
				.Where(p =>
				{
					var bindablePerk = p.GetComponent<BindablePerk>();

					if (bindablePerk == null)
					{
						return false;
					}

					int? index = 0;
					var input = bindablePerk.perkPrefab.GetComponent<AbilityAddActionsToPlayerInput>();
					if (input != null)
					{
						index = input.customBinding.index;
					}

					return index != null && index == perkIndexToChange;
				}).ToList();

			foreach (var go in availablePerks)
			{
				var perk = go.GetComponent<BindablePerk>();

				var button = Instantiate(cheatButtonTemplate, content.transform);
				var btn = button.GetComponent<CheatButton>();
				btn.ButtonAction = () => ChangeBindablePerk(perkIndexToChange, perk);
				btn.ButtonName = perk.PerkName;
			}
		}
	}
}