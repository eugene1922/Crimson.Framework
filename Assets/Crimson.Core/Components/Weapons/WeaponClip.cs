using System;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class WeaponClip
	{
		private int _current;

		public int Current
		{
			get => _current;
			private set
			{
				_current = Mathf.Clamp(value, 0, Capacity);
				OnUpdate?.Invoke();
			}
		}

		public int Capacity { get; private set; }

		public int Count { get; private set; }

		public bool IsEmpty => Capacity > 0 && Current == 0;

		public event Action OnUpdate;

		public WeaponClip()
		{
		}

		public void Setup(int capacity, int count)
		{
			Capacity = capacity;
			Count = count;
			Current = capacity;
		}

		public void Add(int value)
		{
			Count += value;
			OnUpdate?.Invoke();
		}

		public void Reload()
		{
			if (Count == 0)
			{
				return;
			}
			var value = math.max(0, Count - Capacity);
			Count = value;
			Current = value > 0 ? value : Capacity;
		}

		public void Decrease()
		{
			Current--;
		}
	}
}