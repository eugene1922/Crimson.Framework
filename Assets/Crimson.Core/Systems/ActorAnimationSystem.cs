using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Common.Interactions;
using Assets.Crimson.Core.Common.Items;
using Assets.Crimson.Core.Common.Weapons;
using Assets.Crimson.Core.Components;
using Assets.Crimson.Core.Components.Tags;
using Assets.Crimson.Core.Components.Tags.Interactions;
using Assets.Crimson.Core.Components.Tags.Items;
using Assets.Crimson.Core.Components.Tags.Weapons;
using Crimson.Core.Common;
using Crimson.Core.Components;
using System;
using Unity.Entities;
using Unity.Mathematics;
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
		private EntityQuery _reloadAnimationsQuery;
		private EntityQuery _strafeActorsQuery;
		private EntityQuery _meleeQuery;
		private EntityQuery _rangeQuery;
		private EntityQuery _animatorProxyQuery;

		protected override void OnCreate()
		{
			_meleeQuery = GetEntityQuery(
				ComponentType.Exclude<DeadActorTag>(),
				ComponentType.ReadOnly<AnimationMeleeAttackTag>(),
				ComponentType.ReadOnly<Animator>());

			_rangeQuery = GetEntityQuery(
				ComponentType.Exclude<DeadActorTag>(),
				ComponentType.ReadOnly<AnimationRangeAttackTag>(),
				ComponentType.ReadOnly<Animator>());

			_movementQuery = GetEntityQuery(
				ComponentType.ReadOnly<ActorMovementAnimationData>(),
				ComponentType.ReadOnly<Animator>());

			_projectileQuery = GetEntityQuery(
				ComponentType.ReadOnly<ActorProjectileAnimData>(),
				ComponentType.ReadWrite<ActorProjectileThrowAnimTag>(),
				ComponentType.ReadOnly<Animator>());

			_reloadAnimationsQuery = GetEntityQuery(
				ComponentType.ReadOnly<ReloadTag>(),
				ComponentType.ReadWrite<ReloadAnimationData>(),
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

			_animatorProxyQuery = GetEntityQuery(
				ComponentType.ReadOnly<AnimatorProxy>(),
				ComponentType.ReadOnly<Animator>());
		}

		protected override void OnUpdate()
		{
			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			Entities.With(_animatorProxyQuery).ForEach(
				(Entity entity, Transform transform, AnimatorProxy proxy, Animator animator) =>
				{
					var inputData = EntityManager.GetComponentData<PlayerInputData>(entity);
					var movementData = EntityManager.GetComponentData<ActorMovementData>(entity);
					var move = math.normalizesafe(inputData.Move, float2.zero);
					var moveVector = new Vector3(move.x, 0, move.y);
					var angle = Camera.main.transform.eulerAngles.y;
					angle += transform.eulerAngles.y;
					angle %= 360;
					moveVector = Quaternion.AngleAxis(angle, Vector3.down) * moveVector;
					move = new float2(moveVector.x, moveVector.z);
					proxy.RealSpeed.SetValue(animator, move * movementData.MovementSpeed);
					proxy.LookAtDirection.SetValue(animator, new float2(1, 0));

					var startChangeWeapon = EntityManager.HasComponent<StartChangeWeaponAnimTag>(entity);
					if (startChangeWeapon)
					{
						var data = EntityManager.GetComponentData<EquipedWeaponData>(entity);
						proxy.CurrentWeapon.SetValue(animator, data.Current);
						proxy.PreviousWeapon.SetValue(animator, data.Previous);
						proxy.WeaponChange.SetTrigger(animator);
						EntityManager.RemoveComponent<StartChangeWeaponAnimTag>(entity);
					}

					var endChangeWeapon = EntityManager.HasComponent<EndChangeWeaponAnimTag>(entity);
					if (endChangeWeapon)
					{
						var data = EntityManager.GetComponentData<EquipedWeaponData>(entity);
						proxy.PreviousWeapon.SetValue(animator, data.Current);
						EntityManager.RemoveComponent<EndChangeWeaponAnimTag>(entity);
					}
					//TODO: Crouch
					var hasAttack = EntityManager.HasComponent<WeaponAttackTag>(entity);
					proxy.Attacking.SetValue(animator, hasAttack);
					if (hasAttack)
					{
						proxy.AttackType.SetValue(animator, 0);
						EntityManager.RemoveComponent<WeaponAttackTag>(entity);
					}

					var entityHasHit = EntityManager.HasComponent<DamagedActorTag>(entity);
					proxy.Hit.SetValue(animator, entityHasHit);
					if (entityHasHit)
					{
						proxy.HitDirection.SetValue(animator, new float2(0, 1));
					}

					var hasReload = EntityManager.HasComponent<ReloadTag>(entity);
					proxy.Reloading.SetValue(animator, hasReload);
					if (hasReload)
					{
						EntityManager.RemoveComponent<ReloadTag>(entity);
					}
					//TODO:Dodge

					//TODO:Falling

					var startInteract = EntityManager.HasComponent<StartInteractionAnimTag>(entity);
					if (startInteract)
					{
						var data = EntityManager.GetComponentData<InteractionTypeData>(entity);
						proxy.InteractType.SetValue(animator, data.Type);
						proxy.Interact.SetValue(animator, true);
						EntityManager.RemoveComponent<InteractionTypeData>(entity);
						EntityManager.RemoveComponent<StartInteractionAnimTag>(entity);
					}
					var endInteract = EntityManager.HasComponent<EndInteractionAnimTag>(entity);
					if (endInteract)
					{
						proxy.Interact.SetValue(animator, false);
						EntityManager.RemoveComponent<InteractionTypeData>(entity);
						EntityManager.RemoveComponent<EndInteractionAnimTag>(entity);
					}

					var hasDeath = EntityManager.HasComponent<DeadActorTag>(entity);
					if (hasDeath)
					{
						proxy.Death.SetTrigger(animator);
						proxy.IsDead.SetValue(animator, true);
					}

					//TODO:KnockbackStart
					//TODO:KnockbackFly
					//TODO:KnockbackGround
					//TODO:KnockbackStandUp

					//TODO:IdleFun
					//TODO:IdleFundID

					var useItem = EntityManager.HasComponent<UseItemAnimTag>(entity);
					if (useItem)
					{
						var data = EntityManager.GetComponentData<UseItemAnimData>(entity);
						proxy.ItemUseID.SetValue(animator, data.Type);
						proxy.ItemUse.SetTrigger(animator);
						EntityManager.RemoveComponent<UseItemAnimData>(entity);
						EntityManager.RemoveComponent<UseItemAnimTag>(entity);
					}
				});

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
					if (animator.runtimeAnimatorController == null)
					{
						return;
					}
					animator.SetBool(animation.AnimHash, Math.Abs(move.x) > Constants.MIN_MOVEMENT_THRESH || Math.Abs(move.z) > Constants.MIN_MOVEMENT_THRESH);
					animator.SetFloat(animation.SpeedFactorHash,
						animation.SpeedFactorMultiplier * movement.ExternalMultiplier * Math.Max(Math.Abs(move.x), Math.Abs(move.z)));
				});

			Entities.With(_projectileQuery).ForEach(
				(Entity entity, Animator animator, ref ActorProjectileAnimData data) =>
				{
					if (animator == null)
					{
						Debug.LogError("[PROJECTILE THROW ANIMATION SYSTEM] No Animator found!");
						return;
					}

					if (data.AnimHash == 0)
					{
						Debug.LogError("[PROJECTILE THROW ANIMATION SYSTEM] Some hash(es) not found, check your Actor Projectile Component Settings!");
						return;
					}

					if (animator.runtimeAnimatorController != null)
					{
						animator.SetTrigger(data.AnimHash);
					}
					PostUpdateCommands.RemoveComponent<ActorProjectileThrowAnimTag>(entity);
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

					if (animator.runtimeAnimatorController != null)
					{
						animator.SetBool(animation.AnimHash, true);
					}
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

					if (animator.runtimeAnimatorController != null)
					{
						animator.SetBool(animation.AnimHash, true);
					}
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

					if (animator.runtimeAnimatorController != null)
					{
						animator.SetBool(animation.AnimHash, true);
					}
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

					if (animator.runtimeAnimatorController != null)
					{
						animator.SetTrigger(animation.AnimHash);
					}
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

					if (animator.runtimeAnimatorController != null)
					{
						animator.SetBool(animation.AnimHash, aimingAnimData.AimingActive);
					}
				});
		}
	}
}