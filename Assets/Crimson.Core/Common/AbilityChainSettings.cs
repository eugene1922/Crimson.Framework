using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Crimson.Core.Common
{
	[Serializable]
	public struct AbilityChainSettings
	{
		[SerializeField, PropertyOrder(order: 4)]
		public List<MonoBehaviour> Actions;

		[PropertyOrder(order: 1)]
		public float Delay;

		[InfoBox("Zero value is infinity loop")]
		[PropertyOrder(order: 3)]
		public int LoopCount;

		[PropertyOrder(order: 0)]
		public float StartupDelay;
	}
}