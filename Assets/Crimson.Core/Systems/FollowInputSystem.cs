using Assets.Crimson.Core.Components;
using Crimson.Core.Components;
using Unity.Entities;

namespace Assets.Crimson.Core.Systems
{
	public class FollowInputSystem : ComponentSystem
	{
		protected override void OnUpdate()
		{
			Entities.WithAll<AbilityUIFollowInput>().ForEach(
				(AbilityUIFollowInput ability) =>
				{
					ability.Execute();
				});
		}
	}
}