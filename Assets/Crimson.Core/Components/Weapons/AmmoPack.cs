using Crimson.Core.Common;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Weapons
{
	[Serializable]
	public class AmmoPack : IAmmo
	{
		[ValidateInput(nameof(Validate)), OnValueChanged(nameof(ChangeTarget))]
		public MonoBehaviour _target;

		public int _value;

		public IHasComponentName Target => _target as IHasComponentName;

		public int Value => _value;

		private void ChangeTarget()
		{
			if (_target != null && !Validate(_target))
			{
				var weapon = _target.GetComponent<IWeapon>();
				if (weapon == null)
				{
					var throwable = _target.GetComponent<IThrowable>();
					if (throwable != null)
					{
						_target = throwable as MonoBehaviour;
					}
				}
				else
				{
					_target = weapon as MonoBehaviour;
				}
			}
		}

		private bool Validate(MonoBehaviour item)
		{
			var result = item != null && (item is IWeapon || item is IThrowable);
			if (item != null)
			{
			}
			return result;
		}
	}
}