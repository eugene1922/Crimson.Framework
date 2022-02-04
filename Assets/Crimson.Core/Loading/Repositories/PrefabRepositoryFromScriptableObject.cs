using Crimson.Core.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Crimson.Core.Loading.Repositories
{
    [CreateAssetMenu(fileName = "Prefab Repository", menuName = "Game.Framework/Create Prefab Repository", order = 1)]
    public sealed class PrefabRepositoryFromScriptableObject : ScriptableObject, IPrefabRepository
    {
        public PrefabDictionary items = new PrefabDictionary();

        public T Get<T>(string name) where T : Object
        {
            if (items.StringDictionary.TryGetValue(name, out var item))
            {
                return (T)item.asset;
            }

            throw new KeyNotFoundException($"Prefab with name: {name}, not found at prefab repository!");
        }

        public T Get<T>(ushort key) where T : Object
        {
            if (items.UShortDictionary.TryGetValue(key, out var item))
            {
                return (T)item.asset;
            }

            throw new KeyNotFoundException($"Prefab with key '{key}' not found at prefab repository!");
        }

#if UNITY_EDITOR

        [Button]
        private void Fill()
        {
            items = UnityEditor.AssetDatabase.FindAssets("t:Prefab").Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                                                                    .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<Object>)
                                                                    .OrderBy(s => s.name)
                                                                    .ToList();

            UnityEditor.EditorUtility.SetDirty(this);
        }

#endif
    }
}