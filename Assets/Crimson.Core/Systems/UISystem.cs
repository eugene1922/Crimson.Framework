using Assets.Crimson.Core.Components;
using Assets.Crimson.Core.Components.Weapons;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;

namespace Crimson.Core.Systems
{
	public class UISystem : ComponentSystem
	{
		private EntityQuery _newUIQuery;
		private EntityQuery _playerDataQuery;
		private EntityQuery _updateCustomButtonsQuery;

		protected override void OnCreate()
		{
			_newUIQuery = GetEntityQuery(
				ComponentType.ReadOnly<UIReceiver>(),
				ComponentType.ReadOnly<UIReceiverTag>());

			_playerDataQuery = GetEntityQuery(
				ComponentType.ReadOnly<AbilityActorPlayer>());

			_updateCustomButtonsQuery = GetEntityQuery(
				ComponentType.ReadOnly<AbilityActorPlayer>(),
				ComponentType.ReadWrite<NotifyButtonActionExecutedData>());
		}

		protected override void OnUpdate()
		{
			Entities.With(_newUIQuery).ForEach(
				(Entity uiEntity, UIReceiver uiReceiver) =>
				{
					Entities.With(_playerDataQuery).ForEach(
						(Entity playerEntity, AbilityActorPlayer actorPlayer) =>
						{
							if (uiReceiver.Spawner != actorPlayer.Actor ||
								actorPlayer.UIReceiverList.Contains(uiReceiver)) return;

							if (uiReceiver.UIChannelID != actorPlayer.UIChannelID)
							{
								PostUpdateCommands.RemoveComponent<UIReceiverTag>(uiEntity);
								uiReceiver.gameObject.SetActive(false);
								return;
							}

							actorPlayer.UIReceiverList.Add(uiReceiver);
							actorPlayer.ForceUpdatePlayerUIData();
						});
					Entities.WithAll<AbilityInventory>().ForEach((AbilityInventory inventory) =>
					{
						if (!inventory.UIReceiverList.Items.Contains(uiReceiver))
						{
							inventory.UIReceiverList.Items.Add(uiReceiver);
						}
					});

					Entities.WithAll<WeaponSlot>().ForEach((WeaponSlot slot) =>
					{
						if (!slot.UIReceiverList.Items.Contains(uiReceiver))
						{
							slot.UIReceiverList.Items.Add(uiReceiver);
						}
					});
				});

			Entities.With(_updateCustomButtonsQuery).ForEach(
				(Entity entity, AbilityActorPlayer abilityActorPlayer,
					ref NotifyButtonActionExecutedData notifyButtonActionExecutedData) =>
				{
					var data = notifyButtonActionExecutedData;

					abilityActorPlayer.UIReceiverList.ForEach(r =>
						((UIReceiver)r).NotifyButtonActionExecuted(data.ButtonIndex));
					PostUpdateCommands.RemoveComponent<NotifyButtonActionExecutedData>(entity);
				});
		}
	}
}