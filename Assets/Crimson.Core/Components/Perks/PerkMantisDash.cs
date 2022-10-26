using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Perks
{
	[HideMonoScript]
	public class PerkMantisDash : CooldownBehaviour, IActorAbility, IPerkAbility
	{
		public EntityManager _dstManager;
		public Entity _entity;

		[CastToUI(nameof(NotificationText))]
		public string NotificationText;

		private void SetNotification(string value)
		{
			NotificationText = value;
			UIReceiverList.UpdateUIData(nameof(NotificationText));
		}

		[HideInInspector] public UIReceiverList UIReceiverList = new UIReceiverList();

		[TitleGroup("Dash")] public AbilityFindTargetActor AbilityTarget;
		[TitleGroup("Dash")] public float Angle;
		public AbilityBlocker Blocker;
		[TitleGroup("Dash")] public int Charges = 4;
		[TitleGroup("Dash")] public float Cooldown = 5;

		[ValidateInput("MustBeAbility", "Ability MonoBehaviours must derive from IActorAbility!")]
		[Space, TitleGroup("Dash"), PropertyOrder(1)] public List<MonoBehaviour> DashActions;

		[TitleGroup("Dash")] public float DashDelay = .4f;

		[ValidateInput("MustBeAbility", "Ability MonoBehaviours must derive from IActorAbility!")]
		[TitleGroup("DeadlyJump"), PropertyOrder(1)] public List<MonoBehaviour> DeadlyJumpActions;

		[TitleGroup("DeadlyJump"), MinMaxSlider(0, 15, ShowFields = true)]
		public Vector2 DeadlyJumpRange = new Vector2(2, 5);

		[TitleGroup("Dash")] public float MinDistance = 7;
		private int _charges;
		private IEnumerable<IActorAbility> _dashActions;
		private IEnumerable<IActorAbility> _deadlyJumpActions;
		public IActor Actor { get; set; }

		private bool _inDeadlyJumpRange
		{
			get
			{
				if (Target == null)
				{
					return false;
				}

				var distance = Vector3.Distance(Target.transform.position, transform.position);
				var isValidMinDistance = distance >= DeadlyJumpRange.x;
				var isValidMaxDistance = distance <= DeadlyJumpRange.y;

				return isValidMinDistance && isValidMaxDistance;
			}
		}

		private bool _inEndRange => Target != null && Vector3.Distance(Target.transform.position, transform.position) >= 2;
		private bool _inRange => Target != null && Vector3.Distance(Target.transform.position, transform.position) >= MinDistance;
		private Actor Target => AbilityTarget != null ? AbilityTarget.Target : null;

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			_entity = entity;
			Actor = actor;
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			UIReceiverList.Init(this, entity);
			if (!Actor.Abilities.Contains(this))
			{
				Actor.Abilities.Add(this);
			}
			_dashActions = DashActions.Cast<IActorAbility>();
			_deadlyJumpActions = DeadlyJumpActions.Cast<IActorAbility>();
		}

		public void Apply(IActor target)
		{
			if (IsBlocked || !IsEnable || !_dstManager.Exists(target.ActorEntity) || !_inRange)
			{
				return;
			}
			_charges = Charges;
			ApplyActionWithCooldown(Cooldown, ExecuteDash);
		}

		public void Execute()
		{
			Apply(Actor);
		}

		public void Remove()
		{
			if (Actor != null && Actor.AppliedPerks.Contains(this))
			{
				Actor.AppliedPerks.Remove(this);
			}

			Destroy(this);
		}

		private void ExecuteDash()
		{
			Blocker.IsEnable = true;
			if (_charges == 0)
			{
				Blocker.IsEnable = false;
				return;
			}
			if (_inDeadlyJumpRange)
			{
				Blocker.IsEnable = false;
				SetNotification("DeadlyJump");
				foreach (var ability in _deadlyJumpActions)
				{
					ability.Execute();
				}
				return;
			}
			if (!_inEndRange)
			{
				Blocker.IsEnable = false;
				return;
			}
			SetNotification("Dash");
			this.AddAction(ExecuteDash, DashDelay);
			_charges--;
			var direction = Target.transform.position - transform.position;
			direction.y = 0;
			var angle = _charges % 2 == 0 ? Angle : -Angle;
			direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
			var lookToTarget = Quaternion.LookRotation(direction);
			transform.rotation = lookToTarget;
			foreach (var ability in _dashActions)
			{
				ability.Execute();
			}
		}

		private bool MustBeAbility(List<MonoBehaviour> a)
		{
			return !a.Exists(t => !(t is IActorAbility)) || a.Count == 0;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			var targetPosition = transform.position;
			if (Target != null)
			{
				targetPosition = Target.transform.position;
			}
			Gizmos.DrawLine(transform.position, targetPosition);
			Gizmos.DrawWireSphere(targetPosition, .3f);
			Gizmos.DrawWireSphere(targetPosition, MinDistance);

			Gizmos.color = Color.white;
			Gizmos.DrawLine(Vector3.zero, Quaternion.AngleAxis(Angle, Vector3.up) * transform.forward);
			Handles.matrix = transform.localToWorldMatrix;
			Handles.color = Color.green;
			Handles.DrawWireDisc(Vector3.zero, Vector3.up, DeadlyJumpRange.x);
			Handles.DrawWireDisc(Vector3.zero, Vector3.up, DeadlyJumpRange.y);
		}
	}
}