using System;

namespace Assets.Crimson.Core.Components.Weapons
{
	[Serializable]
	public class AmmoPack : IAmmo
	{
		public string _componentName;
		public int _value;
		public string ComponentName => _componentName;
		public int Value => _value;
	}
}