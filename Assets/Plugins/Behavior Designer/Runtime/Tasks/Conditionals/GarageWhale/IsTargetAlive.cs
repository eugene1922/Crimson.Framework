using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor;
using UnityEngine;
using Crimson.Core.Components;
using BehaviorDesigner.Runtime.Tasks.Movement;

namespace Assets.Plugins.Behavior_Designer.Runtime.Tasks.Conditionals.GarageWhale
{
	[TaskCategory("GarageWhale")]
	public class IsTargetAlive : Conditional
	{
		public SharedGameObject Target;

		private GameObject _target;
		private AbilityActorPlayer _abilityActorPlayer;

		public override TaskStatus OnUpdate()
		{
			if (Target.Value == null)
			{
				return TaskStatus.Failure;
			}
			if (_target != Target.Value || _abilityActorPlayer == null)
			{
				_target = Target.Value;
				_abilityActorPlayer = MovementUtility.GetComponentForType<AbilityActorPlayer>(_target);
			}
			if (_abilityActorPlayer == null)
			{
				return TaskStatus.Failure;
			}
			bool isAlive = _abilityActorPlayer.CurrentHealth > 0;

			return isAlive ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}