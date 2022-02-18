using Crimson.Core.AI;
using Crimson.Core.Components;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFramework.Example.AI
{
	[Serializable]
	public class StandBehaviour : IAIBehaviour
	{
		private AIBehaviourSetting _behaviour = null;
		public string[] AdditionalModes => new string[0];
		public bool NeedActions => false;
		public bool NeedCurve => false;
		public bool NeedTarget => false;
		public string XAxis => "";

		public bool Behave(Entity entity, EntityManager dstManager, ref PlayerInputData inputData)
		{
			return true;
		}

		public float Evaluate(Entity entity, AIBehaviourSetting behaviour, AbilityAIInput ai, List<Transform> targets)
		{
			_behaviour = behaviour;

			return Random.value * _behaviour.basePriority;
		}

		public bool SetUp(Entity entity, EntityManager dstManager)
		{
			return true;
		}
	}
}