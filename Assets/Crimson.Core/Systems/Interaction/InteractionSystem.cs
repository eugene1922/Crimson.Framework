using Assets.Crimson.Core.Common.Interactions;
using Assets.Crimson.Core.Components;
using Assets.Crimson.Core.Components.Interaction;
using Assets.Crimson.Core.Components.Tags;
using Assets.Crimson.Core.Components.Tags.Interactions;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Systems.Interaction
{
	public class InteractionSystem : ComponentSystem
	{
		private EntityQuery _buttonQuery;
		private EntityQuery _interactiveEntitiesQuery;
		private Collider[] _results = new Collider[Constants.COLLISION_BUFFER_CAPACITY];

		protected override void OnCreate()
		{
			_interactiveEntitiesQuery = GetEntityQuery(
				ComponentType.ReadOnly<Transform>(),
				ComponentType.ReadOnly<AbilityPlayerInput>(),
				ComponentType.ReadOnly<ActivatedInteractionZoneTag>(),
				ComponentType.ReadOnly<InteractionZone>());

			_buttonQuery = GetEntityQuery(
				ComponentType.ReadOnly<InputExecuteTag>(),
				ComponentType.ReadOnly<AbilityButtonInputRef>()
				);
		}

		protected override void OnUpdate()
		{
			Entities.With(_interactiveEntitiesQuery).ForEach(
				(Entity entity,
				Transform transform,
				AbilityPlayerInput input,
				InteractionZone zone) =>
				{
					var result = Physics.OverlapBoxNonAlloc(zone.Position, zone.Size / 2, _results);
					if (result > 0)
					{
						InteractionItem nearest = null;
						float minimalDistance = -1;
						for (var i = 0; i < _results.Length; i++)
						{
							if (_results[i] == null)
							{
								continue;
							}

							if (nearest == null)
							{
								nearest = _results[i].GetComponent<InteractionItem>();
								if (nearest != null)
								{
									minimalDistance = (transform.position - nearest.transform.position).magnitude;
								}
								continue;
							}

							var item = _results[i].GetComponent<InteractionItem>();
							if (item != null)
							{
								var distance = (transform.position - item.transform.position).magnitude;
								if (distance < minimalDistance)
								{
									nearest = item;
									minimalDistance = distance;
								}
							}
						}
						if (nearest != null)
						{
							EntityManager.AddComponentData(entity, new InteractionTypeData((byte)nearest.Type));
							EntityManager.AddComponentData(entity, new StartInteractionAnimTag());
							nearest.TargetActor = zone.Actor;
							nearest.Execute();
						}
					}
					EntityManager.RemoveComponent<ActivatedInteractionZoneTag>(entity);
				}
			);
			Entities.With(_buttonQuery).ForEach(
				(AbilityButtonInputRef buttonInput) =>
				{
					if (!buttonInput.IsActivated)
					{
						return;
					}
					var actions = buttonInput._actions;
					for (var i = 0; i < actions.Count; i++)
					{
						actions[i]?.Execute();
					}
				}
			);
		}
	}
}