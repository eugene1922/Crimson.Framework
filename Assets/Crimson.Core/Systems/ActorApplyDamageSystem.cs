using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;

namespace Crimson.Core.Systems
{
    public class ActorApplyDamageSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Entities.WithAll<DamageData>().ForEach((Entity damageEntity, ref DamageData damageData) =>
            {
                if (!(dstManager.Exists(damageData.TargetEntity) &&
                      dstManager.Exists(damageData.AbilityOwnerEntity))) return;

                var abilityOwner = dstManager.GetComponentObject<AbilityActorPlayer>(damageData.AbilityOwnerEntity);

                if (dstManager.HasComponent<AbilityActorPlayer>(damageData.TargetEntity) &&
                    !dstManager.HasComponent<DeadActorData>(damageData.TargetEntity) &&
                    !dstManager.HasComponent<DestructionPendingData>(damageData.TargetEntity))
                {
                    var target = dstManager.GetComponentObject<AbilityActorPlayer>(damageData.TargetEntity);

                    if (target.IsAlive)
                    {
                        target.UpdateHealthData((int) -damageData.DamageValue);
                        abilityOwner.UpdateTotalDamageData((int) damageData.DamageValue);
                    }

                    if (!target.IsAlive)
                    {
                        abilityOwner.UpdateExperienceData(GameMeta.PointsForKill);
                    }
                }

                if (dstManager.HasComponent<AbilityDestructibleObject>(damageData.TargetEntity))
                {
                    var target = dstManager.GetComponentObject<AbilityDestructibleObject>(damageData.TargetEntity);

                    if (target != null)
                    {
                        target.UpdateStrengthValue((int) -damageData.DamageValue);
                        abilityOwner.UpdateTotalDamageData((int) damageData.DamageValue);
                    }
                }

                PostUpdateCommands.DestroyEntity(damageEntity);
            });
        }
    }
}