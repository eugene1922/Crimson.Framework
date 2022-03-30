using Sirenix.OdinInspector;
using System;

namespace Assets.Crimson.Core.Common.UI.Widgets.Weapons.Data
{
	[Serializable]
	public struct WeaponSlotData
	{
		[ReadOnly] public string Name;
		[ReadOnly] public int CurrentClips;
		[ReadOnly] public int MaxClips;
	}
}