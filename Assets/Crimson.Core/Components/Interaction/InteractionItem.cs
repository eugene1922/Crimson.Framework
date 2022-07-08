using Assets.Crimson.Core.Common.Types;
using Assets.Crimson.Core.Components.Tags.Interactions;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
	[HideMonoScript]
	public class InteractionItem : MonoBehaviour, IActorAbilityTarget
	{
		public InteractionType Type;
		public float InteractDuration;

		[HideInInspector] public List<IActorAbility> _abilityActions;
		public Entity _entity;
		private EntityManager _entityManager;
		[HideInInspector] public List<IActorAbilityTarget> _targetActions;
		public bool DestroyAfterUse;
		public ActionReciver InteractionReciever;
		public IActor AbilityOwnerActor { get; set; }
		public IActor Actor { get; set; }

		public IActor TargetActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entity = entity;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			AbilityOwnerActor = actor;
			var actions = InteractionReciever.Actions;

			_targetActions = new List<IActorAbilityTarget>();
			_abilityActions = new List<IActorAbility>();

			for (var i = 0; i < actions.Count; i++)
			{
				if (actions[i] is IActorAbilityTarget target)
				{
					_targetActions.Add(target);
				}

				_abilityActions.Add((IActorAbility)actions[i]);
			}
		}

		public void Execute()
		{
			if (TargetActor != null)
			{
				InvokeActions();
			}
			if (DestroyAfterUse)
			{
				GameObjectUtils.DestroyWithEntity(gameObject, _entity);
			}
		}

		private void InvokeActions()
		{
			for (var i = 0; i < _targetActions.Count; i++)
			{
				_targetActions[i].TargetActor = TargetActor;
			}

			for (var i = 0; i < _abilityActions.Count; i++)
			{
				_abilityActions[i]?.Execute();
			}

			var target = TargetActor.ActorEntity;
			_entityManager.AddComponentData(target, new EndInteractionAnimData(InteractDuration));

			TargetActor = null;
		}
	}
}