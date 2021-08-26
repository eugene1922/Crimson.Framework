using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Systems
{
    public class UIActorFollowMovementSystem : ComponentSystem
    {
        private EntityQuery _followingObjectsQuery;

        protected override void OnCreate()
        {
            _followingObjectsQuery = GetEntityQuery(
                ComponentType.ReadOnly<UIActorFollowMovementData>(),
                ComponentType.ReadOnly<RectTransform>());
        }

        protected override void OnUpdate()
        {
            Entities.With(_followingObjectsQuery).ForEach(
                (Entity entity, AbilityUIActorFollowSpawner follow, RectTransform rect) =>
                {
                    if (follow == null || follow.Actor == null || follow.Actor.Spawner.GameObject == null ||
                        follow.Actor.Spawner.GameObject.transform == null || !follow.gameObject.activeSelf ||
                        Camera.main == null) return;

                    if (ReferenceEquals(follow.Actor.Spawner.GameObject, null))
                    {
                        World.EntityManager.AddComponent<ImmediateActorDestructionData>(follow.Actor.ActorEntity);
                        return;
                    }
                    
                    var targetPosition = follow.Actor.Spawner.GameObject.transform.position;

                    var offsetPos = targetPosition + follow.Offset;
                    Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, null,
                        out var canvasPos);

                    follow.RelatedCanvasRect.localPosition = canvasPos;
                }
            );
        }
    }
}