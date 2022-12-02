using Assets.Crimson.Core.Components.Tags.Dialogs;
using Assets.Crimson.Core.Components.UI.Dialogs;
using Unity.Entities;

namespace Assets.Crimson.Core.Systems.UI
{
	public class DialogSystem : ComponentSystem
	{
		private EntityQuery _dialogQuery;

		protected override void OnCreate()
		{
			_dialogQuery = GetEntityQuery(
				ComponentType.ReadOnly<AbilityDialogViewer>());
		}

		protected override void OnUpdate()
		{
			Entities.With(_dialogQuery).ForEach(
				(AbilityDialogViewer ability) =>
				{
					var target = ability.Actor.Spawner.ActorEntity;
					var openedDialog = EntityManager.HasComponent<DialogOpenedTag>(target);
					if (openedDialog != ability.IsVisible)
					{
						ability.IsVisible = openedDialog;
					}

					if (EntityManager.HasComponent<DialogData>(target))
					{
						var data = EntityManager.GetComponentData<DialogData>(target);
						if (ability.Current.ID != data.ID)
						{
							ability.Open(data);
						}
					}
				});
		}
	}
}