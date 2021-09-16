using Crimson.Core.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components.Weapons
{
    [HideMonoScript]
    public class AbilityWeaponBehavior : MonoBehaviour, IActorAbility, IAIModule
    {
        public IActor Actor { get; set; }

        public List<WeaponBehaviourSetting> behaviours;

        [HideInInspector] public WeaponBehaviourSetting activeBehaviour;

        private Entity _entity;
        private EntityManager _dstManager;

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            Actor = actor;
            _entity = entity;
            _dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            _dstManager.AddComponent<NetworkSyncReceive>(entity);

            //for (var i = 0; i < behaviours.Count; i++)
            //{
            //    behaviours[i] = behaviours[i].CopyBehaviour();
            //    behaviours[i].Actor = Actor;
            //}

            EvaluateAll();
        }

        private void Start()
        {
            //var tempBehaviours = new List<AIBehaviourSetting>();

            //foreach (var t in behaviours)
            //{
            //    tempBehaviours.Add(t.CopyBehaviour());
            //}

            //behaviours = tempBehaviours;
        }

        public void EvaluateAll()
        {
            _dstManager.RemoveComponent<SetupAIData>(_entity);
        }

        public void Execute()
        {
        }
    }
}
