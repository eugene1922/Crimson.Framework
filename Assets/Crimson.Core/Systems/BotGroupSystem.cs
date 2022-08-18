using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components;
using Unity.Entities;
using Unity.Mathematics;

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
				(AbilityAggressiveGroup group) =>
				{
					var actors = group.GroupActors;
					foreach (var actor in actors)
					{
						var hasBasePoint = EntityManager.HasComponent<BasePointData>(actor.ActorEntity);
						if (!hasBasePoint)
						{
							continue;
						}
						var basePoint = EntityManager.GetComponentData<BasePointData>(actor.ActorEntity);
						var distance = math.distance(actor.transform.position, basePoint.Position);
						if (distance > group.Distance)
						{
							group.RemoveTag();
						}
					}
				});
		}
	}
}