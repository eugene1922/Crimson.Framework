using Assets.Crimson.Core.Common.Interfaces;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class AbilityPickupWeapon : MonoBehaviour, IActorAbilityTarget
	{
		[ValidateInput(nameof(MustBeWeapon), "MonoBehaviours must derive from IEquipable!")]
		public MonoBehaviour TargetWeapon;

		public IActor AbilityOwnerActor { get; set; }
		public IActor Actor { get; set; }
		public IActor TargetActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			AbilityOwnerActor = actor;
		}

		public void Execute()
		{
			if (TargetActor == null)
			{
				Debug.LogError("Target actor is null", gameObject);
				return;
			}
			if (TargetWeapon == null)
			{
				Debug.LogError("Target weapon is null", gameObject);
				return;
			}

			var equipAbility = TargetWeapon.GetComponent<IEquipable>();
			equipAbility.Equip(TargetActor);
		}

		private bool MustBeWeapon(MonoBehaviour item)
		{
			return item == null || item is IEquipable || item.GetComponent<IEquipable>() != null;
		}
	}
}