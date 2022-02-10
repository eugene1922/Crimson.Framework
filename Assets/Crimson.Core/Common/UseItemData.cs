using Assets.Crimson.Core.Components;
using Unity.Entities;

namespace Assets.Crimson.Core.Common
{
    public struct UseItemData : IComponentData
    {
        public InventoryItemData Item;
    }
}