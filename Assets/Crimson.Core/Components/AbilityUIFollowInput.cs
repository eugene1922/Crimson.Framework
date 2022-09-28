using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	[HideMonoScript]
	public class AbilityUIFollowInput : MonoBehaviour, IActorAbility
	{
		public CanvasGroup CanvasGroup;
		public float HideRadius = .4f;
		public RectTransform Mark;

		private EntityManager _entityManager;
		private Actor _owner;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_owner = (Actor)Actor.Spawner;
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		public void Execute()
		{
			var aimData = _entityManager.GetComponentData<AimData>(_owner.ActorEntity);
			var ownerPosition = Camera.main.WorldToScreenPoint(_owner.transform.position);
			var position = Camera.main.WorldToScreenPoint(aimData.LockedPosition);
			var hasPostion = (ownerPosition - position).magnitude > HideRadius;
			CanvasGroup.alpha = hasPostion ? 1 : 0;
			if (hasPostion)
			{
				Mark.anchoredPosition = position;
			}
		}
	}
}