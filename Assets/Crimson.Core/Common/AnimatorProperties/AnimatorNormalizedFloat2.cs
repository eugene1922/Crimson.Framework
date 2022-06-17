using Sirenix.OdinInspector;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Crimson.Core.Common.AnimatorProperties
{
	[Serializable, HideLabel]
	public struct AnimatorNormalizedFloat2 : IAnimatorProperty<float2>
	{
		public string NameX;
		public string NameY;

		public float2 GetValue(Animator target)
		{
			var valueX = target.GetFloat(NameX);
			var valueY = target.GetFloat(NameY);
			return new float2(valueX, valueY);
		}

		public void SetValue(Animator target, float2 value)
		{
			target.SetFloat(NameX, value.x);
			target.SetFloat(NameY, value.y);
		}
	}
}