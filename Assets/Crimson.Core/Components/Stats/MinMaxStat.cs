using System;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Stats
{
	[Serializable]
	public struct MinMaxStat<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
	{
		[SerializeField] private T _current;

		[SerializeField] private T _maxLimit;

		[SerializeField] private T _minLimit;

		public T Current
		{
			get => _current;
			set
			{
				var isEquals = _current.Equals(value);
				if (isEquals)
				{
					return;
				}
				var newValue = _current;
				if (value.CompareTo(MinLimit) == -1)
				{
					newValue = MinLimit;
				}
				else if (value.CompareTo(MaxLimit) == 1)
				{
					newValue = MaxLimit;
				}
				var isNew = !_current.Equals(newValue);
				if (isNew)
				{
					_current = newValue;
				}
			}
		}

		public T MaxLimit
		{
			get => _maxLimit;
			set
			{
				if (_maxLimit.CompareTo(_minLimit) == 1)
				{
					_maxLimit = value;
				}
			}
		}

		public T MinLimit
		{
			get => _minLimit;
			set
			{
				if (_minLimit.CompareTo(_maxLimit) == -1)
				{
					_minLimit = value;
				}
			}
		}
	}
}