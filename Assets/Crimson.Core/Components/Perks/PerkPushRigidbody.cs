using Assets.Crimson.Core.Components.Forces;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Perks
{
	[HideMonoScript]
	public class PerkPushRigidbody : MonoBehaviour, IActorAbility, IPerkAbility
	{
		public EntityManager _dstManager;
		public Entity _entity;

		public Vector3 Direction;

		public float Value = 25;

		public ForceMode ForceMode;

		public IActor Actor { get; set; }

		private ForceData _force => new ForceData
		{
			Force = Value,
			ForceMode = (int)ForceMode
		};

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

		public void Apply(IActor target)
		{
			if (!target.AppliedPerks.Contains(this))
			{
				target.AppliedPerks.Add(this);
			}
			if (!_dstManager.Exists(target.ActorEntity))
			{
				return;
			}
			var forceData = _force;
			forceData.Direction = transform.TransformDirection(Direction).normalized;
			_dstManager.AddComponent<AdditionalForceActorTag>(target.ActorEntity);
			if (_dstManager.HasComponent<ForceData>(target.ActorEntity))
			{
				_dstManager.SetComponentData(target.ActorEntity, forceData);
			}
			else
			{
				_dstManager.AddComponentData(target.ActorEntity, forceData);
			}
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

		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawLine(Vector3.zero, Direction);
		}
	}
}