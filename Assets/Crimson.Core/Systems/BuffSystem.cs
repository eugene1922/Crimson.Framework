using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.Components;
using Unity.Entities;

namespace Assets.Crimson.Core.Systems
{
	public class BuffSystem : ComponentSystem
	{
		private const float _timeout = 1;
		private EntityQuery _healBuffQuery;

		protected override void OnCreate()
		{
			_healBuffQuery = GetEntityQuery(
				ComponentType.ReadWrite<FullHealPercentBuff>(),
				ComponentType.ReadOnly<AbilityActorPlayer>()
				);
		}

		protected override void OnUpdate()
		{
			Entities.With(_healBuffQuery).ForEach(
				(Entity entity, ref FullHealPercentBuff buff) =>
				{
					var isAggressive = EntityManager.HasComponent<AggressiveAITag>(entity);
					if (isAggressive)
					{
						EntityManager.RemoveComponent<FullHealPercentBuff>(entity);
						return;
					}
					if (buff.Timeout > 0)
					{
						buff.Timeout -= UnityEngine.Time.deltaTime;
						return;
					}
					buff.Timeout = _timeout;
					var player = EntityManager.GetComponentObject<AbilityActorPlayer>(entity);
					if (!player.IsAlive || player.CurrentHealth == player.MaxHealth)
					{
						EntityManager.RemoveComponent<FullHealPercentBuff>(entity);
						return;
					}
					var multiplier = buff.PercentValue / 100;
					var value = player.MaxHealth * multiplier;
					player.UpdateHealth(value);
					if (player.CurrentHealth == player.MaxHealth)
					{
						EntityManager.RemoveComponent<FullHealPercentBuff>(entity);
						return;
					}
				});
		}
	}
}