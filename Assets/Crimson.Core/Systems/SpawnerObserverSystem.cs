using Assets.Crimson.Core.Components;
using Unity.Entities;

namespace Assets.Crimson.Core.Systems
{
	public class SpawnerObserverSystem : ComponentSystem
	{
		protected override void OnUpdate()
		{
			Entities.WithAll<AbilitySpawnerChecker>().ForEach(
				(AbilitySpawnerChecker ability) =>
				{
					if (ability.Actor.Spawner.Equals(null))
					{
						ability.Execute();
					}
				});
		}
	}
}