using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Crimson.Core.Common.AnimatorProperties
{
	[Serializable, HideLabel]
	public struct AnimatorTrigger
	{
		public string Name;

		public void SetTrigger(Animator target)
		{
			target.SetTrigger(Name);
		}
	}
}