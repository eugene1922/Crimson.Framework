using UnityEngine;

namespace Assets.Crimson.Core.Common.AnimatorProperties
{
	public interface IAnimatorProperty<T> : IAnimatorGetter<T>, IAnimatorSetter<T>
	{
		
	}
}