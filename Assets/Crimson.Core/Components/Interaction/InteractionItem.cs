using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
    [HideMonoScript]
    public class InteractionItem : MonoBehaviour, IActorAbility
    {
        public Entity _entity;
        public IActor Actor { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
            _entity = entity;
        }

        public void Execute()
        {
            GameObjectUtils.DestroyWithEntity(gameObject, _entity);
        }
    }
}