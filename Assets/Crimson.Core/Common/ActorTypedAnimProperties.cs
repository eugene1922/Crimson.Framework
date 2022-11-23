using Sirenix.OdinInspector;
using System;

namespace Assets.Crimson.Core.Common
{
	[Serializable, HideLabel]
	public class ActorTypedAnimProperties
	{
		[ShowIf(nameof(HasAnimation)), PropertyOrder(1)]
		public byte AnimationType;

		[ToggleLeft, PropertyOrder(0)] public bool HasAnimation = false;
	}
}