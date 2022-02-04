using Crimson.Core.Common;
using Crimson.Core.Components;
using Crimson.Core.Loading.Repositories;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Assets.Crimson.Core.Components
{
    public struct AddItemData : IComponentData
    {
        public ushort ID;
    }

    public struct RemoveItemData : IComponentData
    {
        public ushort ID;
    }

    public class AbilityPickupItem : MonoBehaviour, IActorAbilityTarget
    {
        public ushort ItemID;
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
            manager.AddComponentData(TargetActor.ActorEntity, new AddItemData() { ID = ItemID });
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
            ItemID = repository.items.GetKey(gameObject);
        }

#endif
    }
}