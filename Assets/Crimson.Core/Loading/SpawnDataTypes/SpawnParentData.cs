using Crimson.Core.Enums;
using UnityEngine;

namespace Assets.Crimson.Core.Loading.SpawnDataTypes
{
	public struct SpawnParentData
	{
		public string ComponentName;
		public string Tag;
		public TargetType Target;
		public ChooseTargetStrategy TargetStrategy;
		public Transform Point;

		public bool IsEmpty => Target == TargetType.None;
	}
}