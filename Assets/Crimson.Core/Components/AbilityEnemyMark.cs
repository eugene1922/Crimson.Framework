using Assets.Crimson.Core.Components.Tags;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public class AbilityEnemyMark : MonoBehaviour, IActorAbility
	{
		public Vector3 PivotOffset;
		public Vector3 LockRange = Vector3.one;

		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, new EnemyData(PivotOffset, LockRange));
		}

		public void Execute()
		{
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawWireCube(transform.position + PivotOffset, LockRange * 2);
		}
	}
}