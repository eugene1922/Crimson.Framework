using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
    public struct ActivatedInteractionZone : IComponentData
    {
    }

    [HideMonoScript]
    public class InteractionZone : MonoBehaviour, IActorAbility
    {
        public EntityManager _dstManager;
        public Vector3 Position;
        public float Radius;
        public IActor Actor { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
            _dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        public void Execute()
        {
            _dstManager.AddComponent<ActivatedInteractionZone>(Actor.ActorEntity);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

#endif
    }
}