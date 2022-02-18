using Crimson.Core.Components;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Common
{
	public interface IActor
	{
		Entity ActorEntity { get; }
		EntityManager WorldEntityManager { get; }
		IActor Spawner { get; set; }
		IActor Owner { get; set; }
		int ActorId { get; set; }
		int ActorStateId { get; }
		ushort ChildrenSpawned { get; set; }

		GameObject GameObject { get; }
		List<string> ComponentNames { get; }
		List<IActorAbility> Abilities { get; }
		List<IPerkAbility> AppliedPerks { get; }

		void PerformSpawnActions();
	}
}