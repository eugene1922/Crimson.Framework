using Crimson.Core.Common;
using Crimson.Core.Components;
using System;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Systems
{
	public class ActorAnimationSystem : ComponentSystem
	{
		private EntityQuery _aimingAnimationQuery;
		private EntityQuery _damagedActorsQuery;
		private EntityQuery _deadActorsQuery;
		private EntityQuery _forceActorsQuery;
		private EntityQuery _movementQuery;
		private EntityQuery _projectileQuery;
		private EntityQuery _strafeActorsQuery;

		protected override void OnCreate()
		{
			_movementQuery = GetEntityQuery(
				ComponentType.ReadOnly<ActorMovementAnimationData>(),
				ComponentType.ReadOnly<Animator>());

			_projectileQuery = GetEntityQuery(
				ComponentType.ReadOnly<ActorProjectileAnimData>(),
				ComponentType.ReadWrite<ActorProjectileThrowAnimData>(),
				ComponentType.ReadOnly<Animator>());

			_deadActorsQuery = GetEntityQuery(
				ComponentType.ReadOnly<DeadActorTag>(),
				ComponentType.ReadWrite<ActorDeathAnimData>(),
				ComponentType.ReadOnly<Animator>());

			_strafeActorsQuery = GetEntityQuery(
				ComponentType.ReadOnly<StrafeActorTag>(),
				ComponentType.ReadWrite<ActorStafeAnimData>(),
				ComponentType.ReadOnly<Animator>());

			_forceActorsQuery = GetEntityQuery(
				ComponentType.ReadOnly<AdditionalForceActorTag>(),
				ComponentType.ReadWrite<ActorForceAnimData>(),
				ComponentType.ReadOnly<Animator>());

			_damagedActorsQuery = GetEntityQuery(
				ComponentType.ReadWrite<DamagedActorTag>(),
				ComponentType.ReadOnly<ActorTakeDamageAnimData>(),
				ComponentType.ReadOnly<Animator>());

			_aimingAnimationQuery = GetEntityQuery(
				ComponentType.ReadOnly<AimingAnimProperties>(),
				ComponentType.ReadOnly<ActorEvaluateAimingAnimData>(),
				ComponentType.ReadOnly<Animator>());
		}

		protected override void OnUpdate()
		{
			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			Entities.With(_movementQuery).ForEach(
				(Entity entity, Animator animator, ref ActorMovementAnimationData animation, ref ActorMovementData movement) =>
				{
					if (animator == null)
					{
						Debug.LogError("[MOVEMENT ANIMATION SYSTEM] No Animator found!");
						return;
					}

					var move = movement.MovementCache;
					if (animation.AnimHash == 0 || animation.SpeedFactorHash == 0)
					{
						Debug.LogError("[MOVEMENT ANIMATION SYSTEM] Some hash(es) not found, check your Actor Movement Component Settings!");
						return;
					}
					animator.SetBool(animation.AnimHash, Math.Abs(move.x) > Constants.MIN_MOVEMENT_THRESH || Math.Abs(move.z) > Constants.MIN_MOVEMENT_THRESH);
					animator.SetFloat(animation.SpeedFactorHash,
						animation.SpeedFactorMultiplier * movement.ExternalMultiplier * Math.Max(Math.Abs(move.x), Math.Abs(move.z)));
				});

			Entities.With(_projectileQuery).ForEach(
				(Entity entity, Animator animator, ref ActorProjectileAnimData animation) =>
				{
					if (animator == null)
					{
						Debug.LogError("[PROJECTILE THROW ANIMATION SYSTEM] No Animator found!");
						return;
					}

					if (animation.AnimHash == 0)
					{
						Debug.LogError("[PROJECTILE THROW ANIMATION SYSTEM] Some hash(es) not found, check your Actor Projectile Component Settings!");
						return;
					}

					animator.SetTrigger(animation.AnimHash);
					PostUpdateCommands.RemoveComponent<ActorProjectileThrowAnimData>(entity);
				});

			Entities.With(_deadActorsQuery).ForEach(
				(Entity entity, Animator animator, ref ActorDeathAnimData animation) =>
				{
					if (animator == null)
					{
						Debug.LogError("[DEATH ANIMATION SYSTEM] No Animator found!");

						dstManager.AddComponent<ImmediateDestructionActorTag>(entity);
						return;
					}

					if (animation.AnimHash == 0)
					{
						Debug.LogError("[DEATH ANIMATION SYSTEM] Some hash(es) not found, check your Actor Death Component Settings!");

						dstManager.AddComponent<ImmediateDestructionActorTag>(entity);
						return;
					}

					animator.SetBool(animation.AnimHash, true);
					dstManager.RemoveComponent<ActorDeathAnimData>(entity);
				});

			Entities.With(_strafeActorsQuery).ForEach(
				(Entity entity, Animator animator, ref ActorStafeAnimData animation) =>
				{
					if (animator == null)
					{
						Debug.LogError("[STRAFE ANIMATION SYSTEM] No Animator found!");
						return;
					}

					if (animation.AnimHash == 0)
					{
						Debug.LogError("[STRAFE ANIMATION SYSTEM] Some hash(es) not found, check your Actor STRAFE Component Settings!");
						return;
					}

					animator.SetBool(animation.AnimHash, true);
					dstManager.RemoveComponent<ActorStafeAnimData>(entity);
				});

			Entities.With(_forceActorsQuery).ForEach(
				(Entity entity, Animator animator, ref ActorForceAnimData animation) =>
				{
					if (animator == null)
					{
						Debug.LogError("[AdditionalForce ANIMATION SYSTEM] No Animator found!");
						return;
					}

					if (animation.AnimHash == 0)
					{
						Debug.LogError("[AdditionalForce ANIMATION SYSTEM] Some hash(es) not found, check your Actor Force Component Settings!");
						return;
					}

					animator.SetBool(animation.AnimHash, true);
					dstManager.RemoveComponent<ActorForceAnimData>(entity);
				});

			Entities.With(_damagedActorsQuery).ForEach(
				(Entity entity, Animator animator, ref ActorTakeDamageAnimData animation, ref DamagedActorTag damagedActorData) =>
				{
					if (animator == null)
					{
						Debug.LogError("[DAMAGE ANIMATION SYSTEM] No Animator found!");
						return;
					}

					if (animation.AnimHash == 0)
					{
						Debug.LogError("[DAMAGE ANIMATION SYSTEM] Some hash(es) not found, check your Actor Damage Component Settings!");
						return;
					}

					animator.SetTrigger(animation.AnimHash);
					dstManager.RemoveComponent<DamagedActorTag>(entity);
				});

			Entities.With(_aimingAnimationQuery).ForEach(
				(Entity entity, Animator animator, ref AimingAnimProperties animation, ref ActorEvaluateAimingAnimData aimingAnimData) =>
				{
					if (animator == null)
					{
						Debug.LogError("[AIMING ANIMATION SYSTEM] No Animator found!");
						return;
					}

					if (animation.AnimHash == 0)
					{
						Debug.LogError("[AIMING ANIMATION SYSTEM] Some hash(es) not found, check your Actor Damage Component Settings!");
						return;
					}

					animator.SetBool(animation.AnimHash, aimingAnimData.AimingActive);
				});
		}
	}
}