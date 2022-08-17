using Assets.Crimson.Core.Components;
using Unity.Entities;

namespace Assets.Crimson.Core.Systems
{
	public class BotGroupSystem : ComponentSystem
	{
		private EntityQuery _aggressiveGroupsQuery;

		protected override void OnCreate()
		{
			_aggressiveGroupsQuery = GetEntityQuery(
				ComponentType.ReadOnly<AbilityAggressiveGroup>()
				);
		}

		protected override void OnUpdate()
		{
			Entities.With(_aggressiveGroupsQuery).ForEach(
				(AbilityAggressiveGroup ability) =>
				{
					var result = ability.IsAllHasTag;
					if (result == AggressiveTagResult.Any)
					{
						ability.Execute();
					}
				});
		}
	}
}