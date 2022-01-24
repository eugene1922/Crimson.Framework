using Assets.Crimson.Core.Components.Interaction;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Systems.Interaction
{
    public class InteractionSystem : ComponentSystem
    {
        private EntityQuery _interactiveEntitiesQuery;
        private Collider[] _results = new Collider[Constants.COLLISION_BUFFER_CAPACITY];

        protected override void OnCreate()
        {
            _interactiveEntitiesQuery = GetEntityQuery(
                ComponentType.ReadOnly<Transform>(),
                ComponentType.ReadOnly<AbilityPlayerInput>(),
                ComponentType.ReadOnly<ActivatedInteractionZone>(),
                ComponentType.ReadOnly<InteractionZone>());
        }

        protected override void OnUpdate()
        {
            Entities.With(_interactiveEntitiesQuery).ForEach(
                (Entity entity,
                Transform transform,
                AbilityPlayerInput input,
                InteractionZone zone) =>
                {
                    var result = Physics.OverlapSphereNonAlloc(transform.position, zone.Radius, _results);
                    if (result > 0)
                    {
                        for (var i = 0; i < _results.Length; i++)
                        {
                            if (_results[i] == null)
                            {
                                continue;
                            }

                            var item = _results[i].GetComponent<InteractionItem>();
                            if (item != null)
                            {
                                item.TargetActor = zone.Actor;
                                item.Execute();
                            }
                        }
                    }
                    EntityManager.RemoveComponent<ActivatedInteractionZone>(entity);
                }
            );
        }
    }
}