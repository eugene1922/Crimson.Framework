using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Crimson.Core.Common.AnimatorProperties
{
	[Serializable, HideLabel]
	public struct AnimatorBool : IAnimatorProperty<bool>
	{
		public string Name;

		public bool GetValue(Animator target)
		{
			return target.GetBool(Name);
		}

		public void SetValue(Animator target, bool value)
		{
			target.SetBool(Name, value);
		}
	}
}