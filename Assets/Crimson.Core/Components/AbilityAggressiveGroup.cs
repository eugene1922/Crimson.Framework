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
	public class AbilityAggressiveGroup : MonoBehaviour, IActorAbility
	{
		public IActorSpawner[] _spawners;
		public float Distance = 10;

		[ValidateInput(nameof(MustBeSpawner))]
		public List<MonoBehaviour> Spawners = new List<MonoBehaviour>();

		private EntityManager _dstManager;
		public IActor Actor { get; set; }

		public IEnumerable<GameObject> GroupObjects => _spawners.SelectMany(s => s.SpawnedObjects);
		public IEnumerable<Actor> GroupActors => GroupObjects.Select(s => s?.GetComponent<Actor>()).Where(s => s != null);

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			_spawners = Spawners.Cast<IActorSpawner>().ToArray();
			_dstManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		[Button]
		public void AddTag()
		{
			var targets = _spawners.SelectMany(s => s.SpawnedObjects);
			foreach (var target in targets)
			{
				AddTagTo(target);
			}
		}

		public void Execute()
		{
			AddTag();
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

		private void AddTagTo(GameObject target)
		{
			var actor = target.GetComponent<Actor>();
			if (actor == null)
			{
				return;
			}

			_dstManager.AddComponentData(actor.ActorEntity, new AggressiveAITag());
		}

		private bool MustBeSpawner(List<MonoBehaviour> behaviours)
		{
			return behaviours == null || behaviours.All(s => s is IActorSpawner);
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
	}
}