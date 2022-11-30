using Crimson.Core.Common;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Weapons
{
	[Serializable]
	public class AmmoPack : IAmmo
	{
		[ValidateInput(nameof(Validate))]
		public MonoBehaviour _target;

		public int _value;
		public int Value => _value;

		public IHasComponentName Target => _target as IHasComponentName;

		private bool Validate(MonoBehaviour item)
		{
			return item != null && item is IHasComponentName;
		}
	}
}