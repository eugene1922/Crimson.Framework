using Crimson.Core.Components;
using Crimson.Core.Utils.LowLevel;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Systems
{
    [UpdateInGroup(typeof(FixedUpdateGroup))]
    public class ActorTurningFollowMovementSystemTransform : ComponentSystem
    {
        private EntityQuery _query;

        protected override void OnCreate()
        {
            _query = GetEntityQuery(
                ComponentType.ReadOnly<Transform>(),
                ComponentType.ReadOnly<ActorMovementData>(),
                ComponentType.ReadOnly<ActorRotationFollowMovementData>(),
                ComponentType.ReadOnly<PlayerInputData>(),
                ComponentType.Exclude<Rigidbody>(),
                ComponentType.Exclude<StopRotationData>());
        }

        protected override void OnUpdate()
        {
            var dt = Time.fixedDeltaTime;

            Entities.With(_query).ForEach((Entity entity,
                                           Transform transform,
                                           ref ActorMovementData movement,
                                           ref PlayerInputData inputData,
                                           ref ActorRotationFollowMovementData rotation) =>
            {
                if (transform == null) return;
                var dir = new Vector3(movement.MovementCache.x, 0, movement.MovementCache.z);
                //TODO: ƒобавить вращение при прицеливании
                //var aimDirection = new Vector3(inputData.AimDirection.x, 0, inputData.AimDirection.y);
                //if (aimDirection.magnitude > 0)
                //{
                //    dir = aimDirection;
                //}
                if (dir == Vector3.zero) return;

                var rot = transform.rotation;
                var newRot = Quaternion.LookRotation(Vector3.Normalize(dir));
                if (newRot == rot) return;
                transform.rotation = Quaternion.Lerp(rot, newRot, dt * rotation.RotationSpeed);
            });
        }
    }
}