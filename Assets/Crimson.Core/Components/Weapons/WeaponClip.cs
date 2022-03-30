using UnityEngine;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class WeaponClip
	{
		public int Current { get; private set; }

		public int Capacity { get; }

		public bool IsEmpty => Capacity > 0 && Current == 0;

		public WeaponClip(int capacity)
		{
			Current = capacity;
			Capacity = capacity;
		}

		public void Reload()
		{
			Current = Capacity;
		}

		public void Decrease()
		{
			Current = Mathf.Clamp(Current - 1, 0, Capacity);
		}
	}
}