using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
    [HideMonoScript]
    public class AbilityLifespan : TimerBaseBehaviour, ILifespan, IActorAbility
    {
        public IActor Actor { get; set; }
        
        public float lifespan = 3f;
        public bool startOnSpawn = true;

        [ValidateInput("MustBeAbility", "Ability MonoBehaviours must derive from IActorAbility!")]
        public MonoBehaviour actionOnDestroy;
        
        private Entity _entity;

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            _entity = entity;
            
            if (startOnSpawn) Execute();
        }

        public void Execute()
        {
            Timer.TimedActions.AddAction(Die,lifespan);
        }

        public void Die()
        {
            if (actionOnDestroy != null) ((IActorAbility)actionOnDestroy).Execute();
            
            this.gameObject.DestroyWithEntity(_entity);
        }

        private bool MustBeAbility(MonoBehaviour a)
        {
            return (a is IActorAbility)||(a is null);
        }
    }
}