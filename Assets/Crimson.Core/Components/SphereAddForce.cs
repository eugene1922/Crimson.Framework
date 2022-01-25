using Crimson.Core.Common;
using Sirenix.OdinInspector;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
    [HideMonoScript]
    public class SphereAddForce : MonoBehaviour,
        IActorAbility
    {
        public EntityManager _dstManager;
        public Entity _entity;

        public float Force = 25;

        public float Radius = 5;

        public IActor Actor { get; set; }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            _entity = entity;
            Actor = actor;
            _dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            if (!Actor.Abilities.Contains(this))
            {
                Actor.Abilities.Add(this);
            }
        }

        public void Execute()
        {
            var actors = GetActorsInRadius(Radius);
            AddForceTo(actors);
        }

        private void AddForceTo(IActor[] actors)
        {
            for (var i = 0; i < actors.Length; i++)
            {
                AddForceTo(actors[i]);
            }
        }

        private void AddForceTo(IActor targetActor)
        {
            var targetRigidbody = targetActor.GameObject.GetComponent<Rigidbody>();

            if (targetRigidbody == null)
            {
                return;
            }

            _dstManager.AddComponent<AdditionalForceActorData>(Actor.ActorEntity);

            targetRigidbody.AddExplosionForce(Force, transform.position, Radius);
        }

        private IActor[] GetActorsInRadius(float radius)
        {
            var results = Physics.OverlapSphere(transform.position, radius);
            return results.Select(s => s.GetComponent<IActor>()).Where(s => s != null).ToArray();
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

#endif
    }
}