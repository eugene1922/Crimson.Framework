using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crimson.Core.Serialization
{
    [Serializable]
    public sealed class PrefabDictionary : ISerializationCallbackReceiver
    {
        public List<PrefabItem> items;

        public Dictionary<int, PrefabItem> HashDictionary { get; } = new Dictionary<int, PrefabItem>();

        public static implicit operator PrefabDictionary(List<GameObject> prefabs)
        {
            return new PrefabDictionary
            {
                items = prefabs.Select(asset => new PrefabItem(asset)).ToList()
            };
        }

        public void Add(GameObject prefab)
        {
            items.Add(new PrefabItem(prefab));
        }

        public void Clear()
        {
            items.Clear();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (HashDictionary.TryGetValue(item.id, out var otherStringItem))
                    {
                        Debug.LogWarning($"Prefab name collision: hash = {item.id}, " +
                                         $"item = {item.name}, " +
                                         $"otherItem = {otherStringItem.name}");
                    }
                    else
                    {
                        HashDictionary.Add(item.id, item);
                    }
                }
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        internal int GetKey(GameObject gameObject)
        {
            var target = items.First(s => s.name == gameObject.name);
            return target.id;
        }

        [Button]
        private void Test()
        {
            var groups = items.GroupBy(s => s.id);
            foreach (var group in groups.Where(s => s.Count() > 1))
            {
                Debug.Log($"id={group.Key}:Count={group.Count()}");
                foreach (var item in group)
                {
                    Debug.Log($"id={item.id}:name={item.name}");
                }
            }
        }
    }
}