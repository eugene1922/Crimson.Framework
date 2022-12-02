using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.UI.Dialogs
{
	public class AbilityDialogViewer : MonoBehaviour, IActorAbility
	{
		[ValidateInput(nameof(MustBeAbility))]
		public MonoBehaviour[] CloseActions;

		public CanvasGroup VisibleControl;
		private ActorAbilityList _closeActions;
		private Entity _entity;
		private EntityManager _entityManager;
		public IActor Actor { get; set; }

		public DialogData Current { get; private set; }

		public bool IsVisible
		{
			get => VisibleControl.alpha == 1
				&& VisibleControl.interactable
				&& VisibleControl.blocksRaycasts;
			set
			{
				VisibleControl.alpha = value ? 1 : 0;
				VisibleControl.interactable = value;
				VisibleControl.blocksRaycasts = value;
			}
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entity = entity;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			_closeActions = new ActorAbilityList(CloseActions);

			if (!Actor.Abilities.Contains(this))
			{
				Actor.Abilities.Add(this);
			}
		}

		public void Close()
		{
			IsVisible = false;
			_closeActions.Execute();
		}

		public void Execute()
		{
		}

		public void Open(DialogData data)
		{
			Current = data;
			IsVisible = true;
		}

		private bool MustBeAbility(MonoBehaviour[] items)
		{
			return items == null || items.All(s => s is IActorAbility);
		}
	}
}