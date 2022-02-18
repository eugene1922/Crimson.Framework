using Crimson.Core.Common;
using Crimson.Core.Utils;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Components
{
	public class TestComponent : TimerBaseBehaviour, IActorAbility
	{
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Debug.Log("ADD COMPONENT DATA");
		}

		private void Start()
		{
			Debug.Log("COMPONENT START");
			Execute();
		}

		public void Execute()
		{
			Debug.Log("COMPONENT EXECUTE");
			this.AddAction(TestAction, 3);
		}

		private void TestAction()
		{
			Debug.Log("TIMER ACTION");
		}
	}
}