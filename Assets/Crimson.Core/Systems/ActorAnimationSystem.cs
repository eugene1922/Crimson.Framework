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
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Crimson.Core.Systems
{
	public class ActorAnimationSystem : ComponentSystem
	{
		private EntityQuery _aimingAnimationQuery;
		private EntityQuery _animatorProxyBaseQuery;
		private EntityQuery _animatorProxyBotQuery;
		private EntityQuery _animatorProxyPlayerQuery;
		private EntityQuery _damagedActorsQuery;
		private EntityQuery _deadActorsQuery;
		private EntityQuery _forceActorsQuery;
		private EntityQuery _movementQuery;
		private EntityQuery _projectileQuery;
		private EntityQuery _ressurectQuery;

		protected override void OnCreate()
		{
			_movementQuery = GetEntityQuery(
				ComponentType.ReadOnly<ActorMovementAnimationData>(),
				ComponentType.ReadOnly<Animator>());

			_projectileQuery = GetEntityQuery(
				ComponentType.ReadOnly<ActorProjectileAnimData>(),
				ComponentType.ReadWrite<ActorProjectileThrowAnimTag>(),
				ComponentType.ReadOnly<Animator>());

			_deadActorsQuery = GetEntityQuery(
				ComponentType.ReadOnly<DeathAnimationTag>(),
				ComponentType.ReadWrite<ActorDeathAnimData>(),
				ComponentType.ReadOnly<Animator>());

			_ressurectQuery = GetEntityQuery(
				ComponentType.ReadWrite<ActorDeathAnimData>(),
				ComponentType.ReadOnly<Animator>(),
				ComponentType.ReadOnly<RessurectAnimationTag>());

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

			_animatorProxyBaseQuery = GetEntityQuery(
				ComponentType.ReadOnly<ActorMovementData>(),
				ComponentType.ReadOnly<PlayerInputData>(),
				ComponentType.ReadOnly<AnimatorProxy>(),
				ComponentType.ReadOnly<Animator>());

			_animatorProxyBotQuery = GetEntityQuery(
				ComponentType.ReadOnly<ActorMovementData>(),
				ComponentType.ReadOnly<PlayerInputData>(),
				ComponentType.ReadOnly<AnimatorProxy>(),
				ComponentType.ReadOnly<Animator>(),
				ComponentType.ReadOnly<NavMeshAgent>());

			_animatorProxyPlayerQuery = GetEntityQuery(
				ComponentType.ReadOnly<ActorMovementData>(),
				ComponentType.ReadOnly<PlayerInputData>(),
				ComponentType.ReadOnly<AnimatorProxy>(),
				ComponentType.ReadOnly<Animator>(),
				ComponentType.Exclude<NavMeshAgent>());
		}

		protected override void OnUpdate()
		{
			var dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			Entities.With(_animatorProxyBaseQuery).ForEach(
				(Entity entity,
				AnimatorProxy proxy,
				ref PlayerInputData inputData,
				ref ActorMovementData movementData) =>
				{
					var animator = proxy.TargetAnimator;
					proxy.LookAtDirection.SetValue(animator, new float2(0, 1));

					var startChangeWeapon = EntityManager.HasComponent<StartChangeWeaponAnimTag>(entity);
					if (startChangeWeapon)
					{
						var data = EntityManager.GetComponentData<EquipedWeaponData>(entity);
						proxy.CurrentWeapon.SetValue(animator, data.Current);
						proxy.PreviousWeapon.SetValue(animator, data.Previous);
						if (data.NeedAnimation)
						{
							proxy.WeaponChange.SetTrigger(animator);
						}

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
					var hasWeaponAttack = EntityManager.HasComponent<WeaponAttackTag>(entity);
					var hasRangeAttack = EntityManager.HasComponent<AnimationRangeAttackTag>(entity);
					var hasMeleeAttack = EntityManager.HasComponent<AnimationMeleeAttackTag>(entity);
					var hasAnyAttack = hasWeaponAttack || hasRangeAttack || hasMeleeAttack;
					if (hasAnyAttack)
					{
						byte attackType = 0;
						if (hasMeleeAttack || hasRangeAttack)
						{
							var data = EntityManager.GetComponentData<EquipedWeaponData>(entity);
							attackType = data.AttackType;
						}
						proxy.AttackType.SetValue(animator, attackType);
						proxy.Attack.SetTrigger(animator);
						if (hasWeaponAttack)
						{
							EntityManager.RemoveComponent<WeaponAttackTag>(entity);
						}
						if (hasRangeAttack)
						{
							EntityManager.RemoveComponent<AnimationRangeAttackTag>(entity);
						}
						if (hasMeleeAttack)
						{
							EntityManager.RemoveComponent<AnimationMeleeAttackTag>(entity);
						}
					}

					var hasHit = EntityManager.HasComponent<DamagedActorTag>(entity);
					proxy.Hit.SetValue(animator, hasHit);
					if (hasHit)
					{
						proxy.HitDirection.SetValue(animator, new float2(0, 1));
						EntityManager.RemoveComponent<DamagedActorTag>(entity);
					}

					var hasReload = EntityManager.HasComponent<ReloadTag>(entity);
					proxy.Reloading.SetValue(animator, hasReload);
					if (hasReload)
					{
						EntityManager.RemoveComponent<ReloadTag>(entity);
					}
					//TODO:Dodge
					var hasStrafe = EntityManager.HasComponent<StrafeActorTag>(entity);
					proxy.Dodge.SetValue(animator, hasStrafe);
					if (hasStrafe)
					{
						EntityManager.RemoveComponent<StrafeActorTag>(entity);
					}

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
					var endInteract = EntityManager.HasComponent<EndInteractionAnimData>(entity);
					if (endInteract)
					{
						var data = EntityManager.GetComponentData<EndInteractionAnimData>(entity);
						if (data.IsExpired)
						{
							proxy.Interact.SetValue(animator, false);
							EntityManager.RemoveComponent<InteractionTypeData>(entity);
							EntityManager.RemoveComponent<EndInteractionAnimData>(entity);
						}
					}

					var hasDeath = EntityManager.HasComponent<DeadActorTag>(entity);
					if (hasDeath)
					{
						proxy.Death.SetTrigger(animator);
						if (animator.GetCurrentAnimatorStateInfo(proxy.DeathLayer).IsTag(proxy.DeathTag))
						{
							proxy.IsDead.SetValue(animator, true);
						}
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

			Entities.With(_animatorProxyBotQuery).ForEach(
				(Entity entity,
				AnimatorProxy proxy,
				ref PlayerInputData inputData,
				ref ActorMovementData movementData) =>
				{
					var transform = proxy.transform;
					var animator = proxy.TargetAnimator;
					var move = math.normalizesafe(inputData.Move, float2.zero);
					var moveVector = new Vector3(move.x, 0, move.y);
					var angle = Vector3.SignedAngle(transform.forward, Vector3.forward, transform.up);
					move = new float2(moveVector.x, moveVector.z);
					proxy.RealSpeed.SetValue(animator, move * movementData.MovementSpeed);
				});

			Entities.With(_animatorProxyPlayerQuery).ForEach(
				(Entity entity,
				AnimatorProxy proxy,
				ref PlayerInputData inputData,
				ref ActorMovementData movementData) =>
				{
					var transform = proxy.transform;
					var animator = proxy.TargetAnimator;
					var move = math.normalizesafe(inputData.Move, float2.zero);
					var moveVector = new Vector3(move.x, 0, move.y);
					var angle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.up);
					moveVector = Quaternion.AngleAxis(inputData.CompensateAngle, Vector3.up) * moveVector;
					moveVector = Quaternion.AngleAxis(angle, Vector3.up) * moveVector;
					var realSpeed = new float2(moveVector.x, moveVector.z);
					proxy.RealSpeed.SetValue(animator, realSpeed * movementData.MovementSpeed);
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

					var anim = animation;
					if (animator.parameters.Any(a => a.nameHash == anim.AnimHash )) animator.SetBool(animation.AnimHash, Math.Abs(move.x) > Constants.MIN_MOVEMENT_THRESH || Math.Abs(move.z) > Constants.MIN_MOVEMENT_THRESH);
					if (animator.parameters.Any(a => a.nameHash == anim.SpeedFactorHash )) animator.SetFloat(animation.SpeedFactorHash,
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
						var d1 = data;
						if (animator.parameters.Any(a => a.nameHash == d1.AnimHash )) animator.SetTrigger(data.AnimHash);
					}
					PostUpdateCommands.RemoveComponent<ActorProjectileThrowAnimTag>(entity);
				});

			Entities.With(_deadActorsQuery).ForEach(
				(Entity entity, Animator animator, ref ActorDeathAnimData animation) =>
				{
					dstManager.RemoveComponent<DeathAnimationTag>(entity);
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
				});

			Entities.With(_ressurectQuery).ForEach(
				(Entity entity, Animator animator, ref ActorDeathAnimData animation, ref RessurectAnimationTag tag) =>
				{
					dstManager.RemoveComponent<RessurectAnimationTag>(entity);
					if (animator == null)
					{
						Debug.LogError("[DEATH ANIMATION SYSTEM] No Animator found!");
						return;
					}

					if (animation.AnimHash == 0)
					{
						Debug.LogError("[DEATH ANIMATION SYSTEM] Some hash(es) not found, check your Actor Death Component Settings!");
						return;
					}

					if (animator.runtimeAnimatorController != null)
					{
						animator.SetBool(animation.AnimHash, false);
					}
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