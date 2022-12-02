using Assets.Crimson.Core.Components.Tags;
using Assets.Crimson.Core.Components.Tags.Dialogs;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.UI.Dialogs
{
	public class AbilityOpenDialog : MonoBehaviour, IActorAbilityTarget
	{
		public int ID;
		private EntityManager _entityManager;
		public IActor AbilityOwnerActor { get; set; }
		public IActor Actor { get; set; }
		public IActor TargetActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		public void Execute()
		{
			if (TargetActor == null)
			{
				return;
			}

			_entityManager.AddComponentData(TargetActor.ActorEntity, new DialogData() { ID = ID });
			_entityManager.AddComponentData(TargetActor.ActorEntity, new DialogOpenedTag());
			_entityManager.AddComponentData(TargetActor.ActorEntity, new StopInputTag());
			_entityManager.AddComponentData(TargetActor.ActorEntity, new StopMovementData());
		}
	}
}