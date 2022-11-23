using Unity.Entities;

namespace Assets.Crimson.Core.Common.Weapons
{
	public struct EquipedWeaponData : IComponentData
	{
		public byte Current;
		public byte Previous;
		public bool NeedAnimation;
		public byte AttackType;
	}
}