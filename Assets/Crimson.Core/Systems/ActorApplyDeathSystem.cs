using System.Linq;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Systems
{
    public class ActorApplyDeathSystem : ComponentSystem
    {
        private EntityQuery _deadUserQuery;
        private EntityQuery _deadUserUiQuery;
        private EntityQuery _destructionActorByTimerQuery;
        private EntityQuery _immediateActorDestructionTransformQuery;
        private EntityQuery _immediateActorDestructionRectTransformQuery;

        protected override void OnCreate()
        {
            _deadUserQuery = GetEntityQuery(ComponentType.ReadOnly<AbilityActorPlayer>(),
                ComponentType.ReadOnly<UserInputData>(),
                ComponentType.ReadWrite<DeadActorData>(),
                ComponentType.Exclude<ImmediateActorDestructionData>(),
                ComponentType.Exclude<DestructionPendingData>());
            
            _deadUserUiQuery = GetEntityQuery(ComponentType.ReadOnly<Actor>(),
                ComponentType.ReadOnly<UIRespawnScreenView>());
            
            _destructionActorByTimerQuery = GetEntityQuery(ComponentType.ReadOnly<AbilityActorPlayer>(),
                ComponentType.ReadWrite<DeadActorData>(),
                ComponentType.Exclude<ImmediateActorDestructionData>(),
                ComponentType.Exclude<DestructionPendingData>());

            _immediateActorDestructionTransformQuery = GetEntityQuery(ComponentType.ReadOnly<ImmediateActorDestructionData>(), 
                ComponentType.ReadOnly<Transform>());
            
            _immediateActorDestructionRectTransformQuery = GetEntityQuery(ComponentType.ReadOnly<ImmediateActorDestructionData>(), 
                ComponentType.ReadOnly<RectTransform>());
        }

        protected override void OnUpdate()
        {
            var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Entities.With(_destructionActorByTimerQuery).ForEach(
                (Entity entity, AbilityActorPlayer actorPlayer) =>
                {
                    if (dstManager.HasComponent<UserInputData>(entity)) return;

                    foreach (var name in actorPlayer.deadActorBehaviour.OnDeathActionsComponentNames)
                    {
                        var ability = actorPlayer.Actor.Abilities.FirstOrDefault(a =>
                            a is IComponentName componentName && componentName.ComponentName.Equals(name));

                        ability?.Execute();
                    }
                    
                    if (actorPlayer.deadActorBehaviour.RemoveInput)
                    {
                        PostUpdateCommands.RemoveComponent<PlayerInputData>(entity);
                    }
                    
                    actorPlayer.gameObject.DeathPhysics(entity, actorPlayer.deadActorBehaviour);
                    
                    if (!actorPlayer.TimerActive) return;
                    
                    actorPlayer.StartDeathTimer();

                    dstManager.AddComponent<DestructionPendingData>(entity);
                    PostUpdateCommands.RemoveComponent<DeadActorData>(entity);
                }
            );

            Entities.With(_immediateActorDestructionTransformQuery).ForEach(
                (Entity entity, Transform obj) =>
                {
                    if (dstManager.HasComponent<TimerComponent>(entity))
                    {
                        var t = dstManager.GetComponentObject<TimerComponent>(entity);
                        t.TimedActions.Add(TimerUtils.CreateAction(t.DestroyWithEntity,0.1f));
                    }
                    else
                    {
                        obj.gameObject.DestroyWithEntity(entity);   
                    }
                });
            
            Entities.With(_immediateActorDestructionRectTransformQuery).ForEach(
                (Entity entity, RectTransform obj) =>
                {
                    if (dstManager.HasComponent<TimerComponent>(entity))
                    {
                        var t = dstManager.GetComponentObject<TimerComponent>(entity);
                        t.TimedActions.Add(TimerUtils.CreateAction(t.DestroyWithEntity,0.1f));
                    }
                    else
                    {
                        obj.gameObject.DestroyWithEntity(entity);   
                    }
                });
        }
    }
}