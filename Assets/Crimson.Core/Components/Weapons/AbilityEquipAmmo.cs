using Assets.Crimson.Core.Common.Interfaces;
using Crimson.Core.Common;
using Crimson.Core.Components;
using Sirenix.OdinInspector;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Assets.Crimson.Core.Components.Weapons
{
	public class AbilityEquipAmmo : MonoBehaviour, IActorAbilityTarget, IEquipable
	{
		[ValidateInput(nameof(MustBeAmmo))]
		public MonoBehaviour[] Packs;

		private IAmmo[] _packs;
		public IActor AbilityOwnerActor { get; set; }
		public IActor Actor { get; set; }
		public IActor TargetActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
			if (Packs == null)
			{
				_packs = Packs.Cast<IAmmo>().ToArray();
			}
		}

		public void Equip(IActor target)
		{
			if (target == null || _packs == null || _packs.Length == 0)
			{
				return;
			}

			var hotkeyWeapon = target.GameObject.GetComponent<HotkeyWeapon>();
			if (hotkeyWeapon != null)
			{
				for (var i = 0; i < _packs.Length; i++)
				{
					hotkeyWeapon.Add(_packs[i]);
				}
			}
		}

		public void Execute()
		{
			Equip(TargetActor);
		}

		private bool MustBeAmmo(MonoBehaviour[] items)
		{
			return items == null || items.All(s => s is IAmmo);
		}
	}
}