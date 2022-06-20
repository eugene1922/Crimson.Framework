using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Crimson.Core.Common.AnimatorProperties
{
	[Serializable, HideLabel]
	public struct AnimatorTypeValue : IAnimatorSetter<int>, IAnimatorSetter<float>
	{
		public string FloatName;
		public string IntName;

		public void SetValue(Animator target, int value)
		{
			target.SetInteger(IntName, value);
			target.SetFloat(FloatName, value);
		}

		public void SetValue(Animator target, float value)
		{
			target.SetInteger(IntName, (int)value);
			target.SetFloat(FloatName, value);
		}
	}
}