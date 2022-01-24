using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
    [HideMonoScript]
    public class InteractionItem : MonoBehaviour, IActorAbilityTarget
    {
        [HideInInspector] public List<IActorAbilityTarget> _actions;
        public Entity _entity;
        public ActionReciver InteractionReciever;
        public IActor AbilityOwnerActor { get; set; }
        public IActor Actor { get; set; }
        public IActor TargetActor { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
            _entity = entity;
            AbilityOwnerActor = actor;
            _actions = InteractionReciever.Actions.ConvertAll(s => s as IActorAbilityTarget);
        }

        public void Execute()
        {
            if (TargetActor == null)
            {
                return;
            }
            for (var i = 0; i < _actions.Count; i++)
            {
                _actions[i].TargetActor = TargetActor;
                _actions[i].Execute();
            }
            TargetActor = null;
        }
    }
}