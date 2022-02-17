using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Loading.Repositories;
using Sirenix.OdinInspector;
using System.Linq;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
	public struct AddItemData : IBufferElementData
	{
		public InventoryItemData Item;
	}

	public struct RemoveItemData : IBufferElementData
	{
		public InventoryItemData Item;
	}

	public class AbilityPickupItem : MonoBehaviour, IActorAbilityTarget
	{
		[HideInInspector] public InventoryItemData ItemData;
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

			var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
			var ability = TargetActor.Abilities.FirstOrDefault(s => s is AbilityInventory);
			if (ability == null)
			{
				Debug.LogError($"No inventory on actor {TargetActor.GameObject.name}", gameObject);
				return;
			}
			var inventory = ability as AbilityInventory;
			inventory.Add(ItemData);
		}

#if UNITY_EDITOR

		[Button]
		private void Setup()
		{
			var guids = AssetDatabase.FindAssets($"t: {nameof(PrefabRepositoryFromScriptableObject)}");
			if (guids.Length == 0)
			{
				Debug.LogError("Please create prefab repository for correct work", gameObject);
				return;
			}
			var path = AssetDatabase.GUIDToAssetPath(guids[0]);
			var repository = AssetDatabase.LoadAssetAtPath<PrefabRepositoryFromScriptableObject>(path);
			ItemData.ID = repository.items.GetKey(gameObject);
		}

#endif
	}
}