using Crimson.Core.AI;
using Crimson.Core.Common;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Crimson.Core.Components
{
	[HideMonoScript]
	public class AbilityAIInput : TimerBaseBehaviour, IActorAbility, IAIModule
	{
		public IActor Actor { get; set; }

		public List<AIBehaviourSetting> behaviours;

		[MinMaxSlider(0, 30, true)] public Vector2 behaviourUpdatePeriod = new Vector2(2f, 6f);

		[HideInInspector] public AIBehaviourSetting activeBehaviour;
		[HideInInspector] public float activeBehaviourPriority = 0;

		private Entity _entity;
		private EntityManager _dstManager;

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_entity = entity;
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			_dstManager.AddComponent<NetworkSyncReceive>(entity);

			var tempBehaviours = new List<AIBehaviourSetting>();

			foreach (var t in behaviours)
			{
				tempBehaviours.Add(t.CopyBehaviour());
			}

			behaviours = tempBehaviours;

			for (var i = 0; i < behaviours.Count; i++)
			{
				behaviours[i] = behaviours[i].CopyBehaviour();
				behaviours[i].Actor = Actor;
			}

			StartTimer();
			EvaluateAll();
		}

		public void EvaluateAll()
		{
			this.RemoveAction(EvaluateAll);

			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			if (_dstManager.Exists(_entity))
			{
				_dstManager.RemoveComponent<SetupAITag>(_entity);
				_dstManager.AddComponent<EvaluateAITag>(_entity);
			}

			if (TimerActive)
			{
				this.AddAction(EvaluateAll, Random.Range(behaviourUpdatePeriod[0], behaviourUpdatePeriod[1]));
			}
		}

		public void Execute()
		{
		}
	}
}