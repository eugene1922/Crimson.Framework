using Crimson.Core.Components;
using Crimson.Core.Utils.LowLevel;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Systems
{
	[UpdateInGroup(typeof(FixedUpdateGroup))]
	public class TestSystem : ComponentSystem
	{
		private EntityQuery _query;

		protected override void OnCreate()
		{
			_query = GetEntityQuery(
				ComponentType.ReadOnly<TestComponent>());
		}

		protected override void OnUpdate()
		{
			var dt = Time.DeltaTime;

			var c = 0;

			Entities.With(_query).ForEach(
				(Entity entity) =>
				{
					c++;
				});
			Debug.Log("C is " + c);
		}
	}
}