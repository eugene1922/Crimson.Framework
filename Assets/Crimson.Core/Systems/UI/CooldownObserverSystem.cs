using Crimson.Core.Common;
using Unity.Entities;

namespace Assets.Crimson.Core.Systems.UI
{
	public class CooldownObserverSystem : ComponentSystem
	{
		protected override void OnUpdate()
		{
			Entities.WithAll<UIReceiver>().ForEach(
				(UIReceiver receiver) =>
				{
					var observers = receiver.UIObservers;
					for (var i = 0; i < observers.Count; i++)
					{
						observers[i].Refresh(receiver.Spawner);
					}
				});
		}
	}
}