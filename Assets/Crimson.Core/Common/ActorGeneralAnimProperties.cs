using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Crimson.Core.Common
{
	[Serializable]
	public class ActorGeneralAnimProperties
	{
		[ShowIf(nameof(HasAnimation) + " == true"), PropertyOrder(1)]
		public string AnimationName;

		[ToggleLeft, PropertyOrder(0)] public bool HasAnimation = false;

		public int AnimationHash => Animator.StringToHash(AnimationName);
	}
}