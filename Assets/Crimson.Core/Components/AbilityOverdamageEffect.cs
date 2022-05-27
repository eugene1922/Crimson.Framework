using Assets.Crimson.Core.Common;
using Assets.Crimson.Core.Common.ComponentDatas;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityOverdamageEffect : MonoBehaviour, IActorAbility, IHasComponentName
	{
		[SerializeField]
		public string componentName = "";

		public bool NeedDestroyWhenTriggered;

		public float TriggerDamage = 100;

		public ActionsList Actions = new ActionsList();

		private EntityManager _dstManager;
		private Entity _entity;
		public IActor Actor { get; set; }
		public string ComponentName { get => componentName; set => componentName = value; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			_entity = entity;
			Actions.Init();
		}

		public void Execute()
		{
			if (!_dstManager.HasComponent<OverdamageData>(_entity))
			{
				return;
			}

			var overdamageData = _dstManager.GetComponentData<OverdamageData>(_entity);
			if (overdamageData.Damage >= TriggerDamage)
			{
				Actions.Execute();

				if (NeedDestroyWhenTriggered)
				{
					if (_dstManager.HasComponent<AbilityActorPlayer>(_entity))
					{
						var actorPlayer = _dstManager.GetComponentObject<AbilityActorPlayer>(_entity);
						actorPlayer.Death();
					}
				}
			}
		}
	}
}