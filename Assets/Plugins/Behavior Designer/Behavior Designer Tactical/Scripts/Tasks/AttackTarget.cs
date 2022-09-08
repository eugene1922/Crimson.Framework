using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Plugins.Behavior_Designer.Behavior_Designer_Tactical.Scripts.Tasks
{
	[TaskCategory("Tactical")]
	[TaskIcon("Assets/Behavior Designer Tactical/Editor/Icons/{SkinColor}AttackIcon.png")]
	public class AttackTarget : Action
	{
		public SharedGameObject Target;
		public SharedInt AttackInput = 1;
		private const float AIM_MAX_DIST = 40f;
		private AbilityPlayerInput _input;
		private Actor _actor;
		private EntityManager _entityManager;

		public override void OnAwake()
		{
			_input = GetComponent<AbilityPlayerInput>();
			_actor = GetComponent<Actor>();
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			base.OnAwake();
		}

		public override TaskStatus OnUpdate()
		{
			if (Target.Value == null)
			{
				return TaskStatus.Failure;
			}
			var target = Target.Value;
			transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

			if (Physics.Raycast(GetDirectionRay(target.transform), out var hit, AIM_MAX_DIST) && hit.transform == target.transform)
			{
				var data = _entityManager.GetComponentData<PlayerInputData>(_actor.ActorEntity);
				data.CustomInput[AttackInput.Value] = 1f;
				_entityManager.SetComponentData(_actor.ActorEntity, data);
				return TaskStatus.Success;
			}

			return TaskStatus.Running;
		}

		private Ray GetDirectionRay(Transform target)
		{
			return new Ray()
			{
				origin = transform.position + VIEW_POINT_DELTA,
				direction = target.position - transform.position
			};
		}

		private readonly Vector3 VIEW_POINT_DELTA = new Vector3(0f, 0.6f, 0f);
	}
}