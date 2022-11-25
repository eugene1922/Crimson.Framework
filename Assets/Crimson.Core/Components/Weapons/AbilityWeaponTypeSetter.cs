using Assets.Crimson.Core.Common.Types;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class AbilityWeaponTypeSetter : MonoBehaviour, IActorAbility
	{
		public bool ExecuteOnAwake;
		public WeaponType WeaponType;
		public IActor Actor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			if (ExecuteOnAwake)
			{
				Execute();
			}
		}

		public void Awake()
		{
		}

		public void Execute()
		{
			var proxy = GetComponent<AnimatorProxy>();
			proxy.CurrentWeapon.SetValue(proxy.TargetAnimator, (float)WeaponType);
		}
	}
}