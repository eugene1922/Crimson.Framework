using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Utils
{
    public static class ActorUtils
    {
        public static void ChangeActorForceMovementData(this IActor target, Vector3 forwardVector)
        {
            if (target == null) return;

            if (!World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<ActorForceMovementData>(target.ActorEntity)) return;

            var actorForceMovementData = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ActorForceMovementData>(target.ActorEntity);

            actorForceMovementData.MoveDirection = MoveDirection.UseDirection;
            actorForceMovementData.ForwardVector = forwardVector;
            actorForceMovementData.CompensateSpawnerRotation = false;

            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(target.ActorEntity, actorForceMovementData);
        }
    }
}