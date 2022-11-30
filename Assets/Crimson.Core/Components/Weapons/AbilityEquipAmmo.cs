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
		public AmmoPack[] Packs;

		public IActor AbilityOwnerActor { get; set; }
		public IActor Actor { get; set; }
		public IActor TargetActor { get; set; }

		public void AddComponentData(ref Entity entity, IActor actor)
		{
			Actor = actor;
		}

		public void Equip(IActor target)
		{
			if (target == null || Packs == null || Packs.Length == 0)
			{
				return;
			}

			var hotkeyWeapon = target.Abilities.Find(s => s is HotkeyWeapon) as HotkeyWeapon;
			if (hotkeyWeapon != null)
			{
				for (var i = 0; i < Packs.Length; i++)
				{
					hotkeyWeapon.Add(Packs[i]);
				}
			}
		}

		public void Execute()
		{
			Equip(TargetActor);
		}
	}
}