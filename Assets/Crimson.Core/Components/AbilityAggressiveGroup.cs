using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public enum AggressiveTagResult
	{
		None,
		Any,
		All
	}

	public class AbilityAggressiveGroup : MonoBehaviour, IActorAbility
	{
		public IActorSpawner[] _spawners;

		[ValidateInput(nameof(MustBeSpawner))]
		public List<MonoBehaviour> Spawners = new List<MonoBehaviour>();

		private EntityManager _dstManager;
		public IActor Actor { get; set; }

		public AggressiveTagResult IsAllHasTag
		{
			get
			{
				var result = CheckSpawners(_spawners);
				return result;
			}
		}

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_spawners = Spawners.Cast<IActorSpawner>().ToArray();
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		public void AddTag()
		{
			var targets = _spawners.SelectMany(s => s.SpawnedObjects);
			foreach (var target in targets)
			{
				AddTagTo(target);
			}
		}

		[Button]
		public void RemoveTag()
		{
			var targets = _spawners.SelectMany(s => s.SpawnedObjects);
			foreach (var target in targets)
			{
				RemoveFrom(target);
			}
		}

		public void Execute()
		{
			AddTag();
		}

		private void AddTagTo(GameObject target)
		{
			var actor = target.GetComponent<Actor>();
			if (actor == null)
			{
				return;
			}

			_dstManager.AddComponentData(actor.ActorEntity, new AggressiveAITag());
		}

		private void RemoveFrom(GameObject target)
		{
			var actor = target.GetComponent<Actor>();
			if (actor == null)
			{
				return;
			}

			_dstManager.RemoveComponent<AggressiveAITag>(actor.ActorEntity);
		}

		private bool CheckActor(GameObject item)
		{
			var actor = item.GetComponent<Actor>();
			return actor != null && _dstManager.HasComponent<AggressiveAITag>(actor.ActorEntity);
		}

		private AggressiveTagResult CheckObjects(IEnumerable<GameObject> objects)
		{
			var result = AggressiveTagResult.None;
			var targets = objects.Where(s => !s.Equals(null));
			var hasTag = false;
			foreach (var target in targets)
			{
				hasTag = CheckActor(target);
				if (hasTag)
				{
					break;
				}
			}
			if (hasTag)
			{
				result = AggressiveTagResult.Any;
			}
			return result;
		}

		private AggressiveTagResult CheckSpawners(IActorSpawner[] spawners)
		{
			var result = AggressiveTagResult.None;
			var targets = spawners.SelectMany(s => s.SpawnedObjects);
			var spawnerResult = CheckObjects(targets);
			if (spawnerResult == AggressiveTagResult.Any
				|| (result != spawnerResult))
			{
				result = AggressiveTagResult.Any;
			}
			return result;
		}

		private bool MustBeSpawner(List<MonoBehaviour> behaviours)
		{
			return behaviours == null || behaviours.All(s => s is IActorSpawner);
		}
	}
}