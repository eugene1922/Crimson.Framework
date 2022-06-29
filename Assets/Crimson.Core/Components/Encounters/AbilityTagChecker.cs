using Assets.Crimson.Core.Common;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Encounters
{
	public class AbilityTagChecker : TimerBaseBehaviour, IActorAbility, IEnableable
	{
		public bool ExecuteOnAwake;

		public float UpdateTime;

		public float CheckupDelay;

		[ValueDropdown(nameof(GetComponentTypes))]
		public string TagName;

		[ValidateInput(nameof(MustBeSpawner))]
		public List<MonoBehaviour> Spawners = new List<MonoBehaviour>();

		[Header("On Start")]
		public ActionsList ActionsOnStart = new ActionsList();

		[Header("On Success")]
		public ActionsList ActionsOnEnd = new ActionsList();

		private IActorSpawner[] _spawners;
		private EntityManager _dstManager;
		private Type _checkType;

		public IActor Actor { get; set; }
		public bool IsEnable { get; set; } = false;

		private Type[] Types => Assembly.GetExecutingAssembly().GetTypes();

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			if (!string.IsNullOrEmpty(TagName))
			{
				_checkType = Types.First(s => s.Name.Equals(TagName));
			}
			ActionsOnStart.Init();
			ActionsOnEnd.Init();
			_spawners = Spawners.Cast<IActorSpawner>().ToArray();
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			StartTimer();
			if (ExecuteOnAwake)
			{
				Execute();
			}
		}

		[Button]
		public void Execute()
		{
			if (IsEnable)
			{
				return;
			}
			IsEnable = true;
			ActionsOnStart.Execute();
			Timer.TimedActions.AddAction(Check, CheckupDelay);
		}

		private void Check()
		{
			if (IsEnable)
			{
				var result = CheckSpawners(_spawners);
				if (result)
				{
					IsEnable = false;
					ActionsOnEnd.Execute();
				}
				else
				{
					Timer.TimedActions.AddAction(Check, UpdateTime);
				}
			}
		}

		private bool CheckObjects(List<GameObject> objects)
		{
			var result = true;
			for (var i = 0; i < objects.Count; i++)
			{
				var item = objects[i];
				result = item.Equals(null) || CheckActor(item);
				if (!result)
				{
					break;
				}
			}
			return result;
		}

		private bool CheckActor(GameObject item)
		{
			var actor = item.GetComponent<Actor>();
			return actor != null && (_checkType.Equals(null) || _dstManager.HasComponent(actor.ActorEntity, _checkType));
		}

		private bool CheckSpawners(IActorSpawner[] spawners)
		{
			var result = true;
			for (var i = 0; i < spawners.Length; i++)
			{
				var objects = spawners[i].SpawnedObjects;
				result &= CheckObjects(objects);
				if (!result)
				{
					break;
				}
			}
			return result;
		}

		private string[] GetComponentTypes()
		{
			return Types
				.Where(s => s.GetInterfaces().Contains(typeof(IComponentData)))
				.Select(s => s.Name)
				.ToArray();
		}

		private bool MustBeSpawner(List<MonoBehaviour> behaviours)
		{
			return behaviours == null || behaviours.All(s => s is IActorSpawner);
		}
	}
}