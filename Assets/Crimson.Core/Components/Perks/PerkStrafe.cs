using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Enums;
using Crimson.Core.Loading;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Perks
{
    [HideMonoScript]
    public class PerkStrafe : CooldownBehaviour,
        IActorAbility,
        IPerkAbility,
        ICooldownable
    {
        public EntityManager _dstManager;
        public Entity _entity;
        public List<GameObject> _perkEffects = new List<GameObject>();
        public ActorGeneralAnimProperties AnimProperties;
        public float cooldownTime;
        public float force = 25;
        public List<MonoBehaviour> perkRelatedComponents = new List<MonoBehaviour>();
        public float timer = 0.1f;
        private Vector3 _previousVelocity;
        private IActor _target;
        public IActor Actor { get; set; }

        public float CooldownTime
        {
            get => cooldownTime;
            set => cooldownTime = value;
        }

        public void AddComponentData(ref Entity entity, IActor actor)
        {
            _entity = entity;
            Actor = actor;
            _dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            if (AnimProperties.HasAnimation)
            {
                _dstManager.AddComponentData(entity, new ActorStafeAnimData
                {
                    AnimHash = Animator.StringToHash(AnimProperties.AnimationName)
                });
            }

            if (!Actor.Abilities.Contains(this))
            {
                Actor.Abilities.Add(this);
            }
        }

        public void Apply(IActor target)
        {
            _target = target;

            if (!Enabled)
            {
                return;
            }

            ApplyActionWithCooldown(cooldownTime, Strafe);
        }

        public void Execute()
        {
            Apply(Actor);
        }

        public override void FinishTimer()
        {
            base.FinishTimer();
            this.FinishAbilityCooldownTimer(Actor);
        }

        public void Remove()
        {
            if (_target != null && _target.AppliedPerks.Contains(this))
            {
                _target.AppliedPerks.Remove(this);
            }

            foreach (var component in perkRelatedComponents)
            {
                Destroy(component);
            }

            Destroy(this);
        }

        public override void StartTimer()
        {
            base.StartTimer();
            this.StartAbilityCooldownTimer(Actor);
        }

        public void Strafe()
        {
            if (_target != Actor.Owner)
            {
                var ownerActorPlayer =
                    Actor.Owner.Abilities.FirstOrDefault(a => a is AbilityActorPlayer) as AbilityActorPlayer;

                if (ownerActorPlayer == null)
                {
                    return;
                }
            }

            if (!_target.AppliedPerks.Contains(this))
            {
                _target.AppliedPerks.Add(this);
            }

            var targetRigidbody = _target.GameObject.GetComponent<Rigidbody>();

            if (targetRigidbody == null)
            {
                return;
            }

            _dstManager.AddComponent<StrafeActorTag>(Actor.ActorEntity);

            _previousVelocity = targetRigidbody.velocity;
            var dashVector = -1 * _target.GameObject.transform.forward;
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var movement = entityManager.GetComponentData<ActorMovementData>(_target.ActorEntity);
            movement.Input = dashVector;
            entityManager.SetComponentData(_target.ActorEntity, movement);
            targetRigidbody.AddForce(dashVector * force, ForceMode.Impulse);
            if (_perkEffects != null && _perkEffects.Count > 0)
            {
                var spawnData = new ActorSpawnerSettings
                {
                    objectsToSpawn = _perkEffects,
                    SpawnPosition = SpawnPosition.UseSpawnerPosition,
                    parentOfSpawns = TargetType.None,
                    runSpawnActionsOnObjects = true,
                    destroyAbilityAfterSpawn = true
                };

                var fx = ActorSpawn.Spawn(spawnData, Actor, null)?[0];
            }

            Timer.TimedActions.AddAction(() =>
            {
                if (targetRigidbody == null)
                {
                    return;
                }

                targetRigidbody.velocity = _previousVelocity;
            }, timer);
        }
    }
}