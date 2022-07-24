using Assets.Crimson.Core.Common.Interfaces;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Loading.SpawnDataTypes;
using Sirenix.OdinInspector;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class AbilityPickupEquipable : MonoBehaviour, IActorAbilityTarget
	{
		[ValidateInput(nameof(MustBeEquipable), "MonoBehaviours must derive from IEquipable!"), OnValueChanged(nameof(ChangeTarget))]
		public MonoBehaviour TargetWeapon;

		private SpawnItemData _spawnData;

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

			_spawnData = new SpawnItemData()
			{
				Owner = TargetActor,
				Spawner = TargetActor,
				Prefab = TargetWeapon.gameObject,
				SampledComponents = new Component[0]
			};

			EquipWeapon();
		}

		private void EquipWeapon()
		{
			var spawnedWeapon = ActorSpawn.Spawn(_spawnData);
			var equipAbility = spawnedWeapon.GetComponent<IEquipable>();
			equipAbility.Equip(TargetActor);
		}

		private bool MustBeEquipable(MonoBehaviour item)
		{
			return item != null && item is IEquipable;
		}

		private void ChangeTarget()
		{
			if (TargetWeapon != null && !MustBeEquipable(TargetWeapon))
			{
				TargetWeapon = TargetWeapon.GetComponentsInChildren<MonoBehaviour>().FirstOrDefault(s => s is IEquipable);
			}
		}

#if UNITY_EDITOR

		private void OnValidate()
		{
			ChangeTarget();
			if (TargetWeapon == null)
			{
				TargetWeapon = GetComponents<MonoBehaviour>().FirstOrDefault(s => s is IEquipable);
			}
		}

#endif
	}
}