using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Tags.Interactions
{
	public struct EndInteractionAnimData : IComponentData
	{
		public float StartTime { get; }

		public float Duration;

		public bool IsExpired => Time.realtimeSinceStartup - StartTime > Duration;

		public EndInteractionAnimData(float duration)
		{
			StartTime = Time.realtimeSinceStartup;
			Duration = duration;
		}
	}
}