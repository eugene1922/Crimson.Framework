using Crimson.Core.Common;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
    public class AbilityDestructibleObject : MonoBehaviour, IActorAbility
    {
        [ReadOnly] public int currentStrengthValue;
        public int maxStrengthValue;
        public IActor Actor { get; set; }
        
        private EntityManager _dstManager;
        
        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
            
            _dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            currentStrengthValue = maxStrengthValue;

            _dstManager.AddComponentData(entity, new DestructibleObjectStateData
            {
                CurrentStrengthValue = currentStrengthValue,
                MaxStrengthValue = maxStrengthValue
            });
        }

        public void UpdateStrengthValue(int delta)
        {
            var objectState = _dstManager.GetComponentData<DestructibleObjectStateData>(Actor.ActorEntity);

            var newStrength = objectState.CurrentStrengthValue + delta;

            objectState.CurrentStrengthValue =
                newStrength < 0 ? 0 : newStrength > objectState.MaxStrengthValue ? objectState.MaxStrengthValue : newStrength;
            
            currentStrengthValue = objectState.CurrentStrengthValue;

            _dstManager.SetComponentData(Actor.ActorEntity, objectState);

            //UpdateUIData(nameof(CurrentHealth));

            if (currentStrengthValue > 0) return;

            _dstManager.AddComponent<ImmediateDestructionActorTag>(Actor.ActorEntity);
        }

        public void Execute()
        {
            
        }
    }
    
    public struct DestructibleObjectStateData : IComponentData
    {
        public int CurrentStrengthValue;
        public int MaxStrengthValue;
    }
}