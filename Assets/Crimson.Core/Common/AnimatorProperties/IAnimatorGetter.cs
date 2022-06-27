using UnityEngine;

namespace Assets.Crimson.Core.Common.AnimatorProperties
{
	public interface IAnimatorGetter<T>
	{
		public T GetValue(Animator target);
	}
}