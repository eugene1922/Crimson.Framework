using UnityEngine;

namespace Assets.Crimson.Core.Common.AnimatorProperties
{
	public interface IAnimatorSetter<T>
	{
		public void SetValue(Animator target, T value);
	}
}