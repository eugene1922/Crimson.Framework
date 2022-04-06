using Crimson.Core.AI;
using Crimson.Core.Components;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.AI
{
	internal class FallbackBehaviour : IAIBehaviour
	{
		public string XAxis => throw new System.NotImplementedException();

		public string[] AdditionalModes => throw new System.NotImplementedException();

		public bool NeedCurve => throw new System.NotImplementedException();

		public bool NeedTarget => throw new System.NotImplementedException();

		public bool NeedActions => throw new System.NotImplementedException();

		public bool HasDistanceLimit => throw new System.NotImplementedException();

		public bool Behave(Entity entity, EntityManager dstManager, ref PlayerInputData inputData)
		{
			throw new System.NotImplementedException();
		}

		public float Evaluate(Entity entity, AIBehaviourSetting behaviourSetting, AbilityAIInput ai, List<Transform> targets)
		{
			throw new System.NotImplementedException();
		}

		public bool SetUp(Entity entity, EntityManager dstManager)
		{
			throw new System.NotImplementedException();
		}
	}
}