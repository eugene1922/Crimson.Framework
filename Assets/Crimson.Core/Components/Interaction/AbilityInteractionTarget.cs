using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Common.Types;
using Assets.Crimson.Core.Components.Tags.Interactions;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Interaction
{
	[HideMonoScript]
	public class AbilityInteractionTarget : MonoBehaviour, IActorAbilityTarget
	{
		[PropertyOrder(0)] public bool DestroyAfterUse;
		[PropertyOrder(2)] public float InteractDuration;
		[PropertyOrder(3)] public ActionReciver InteractionReciever;
		[PropertyOrder(1)] public InteractionType Type;
		private ActorAbilityList _abilities;
		private Entity _entity;
		private EntityManager _entityManager;
		private ActorAbilityTargetList _targetAbilities;
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

			_abilities = new ActorAbilityList(actions);
			_targetAbilities = new ActorAbilityTargetList(actions);
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
			_targetAbilities.Execute(TargetActor);
			_abilities.Execute();

			var target = TargetActor.ActorEntity;
			_entityManager.AddComponentData(target, new EndInteractionAnimData(InteractDuration));

			TargetActor = null;
		}
	}
}