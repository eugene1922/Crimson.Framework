using Crimson.Core.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crimson.Core.Loading.Repositories
{
	[CreateAssetMenu(fileName = "Prefab Repository", menuName = "Game.Framework/Create Prefab Repository", order = 1)]
	public sealed class PrefabRepositoryFromScriptableObject : ScriptableObject, IPrefabRepository
	{
		public PrefabDictionary items = new PrefabDictionary();

		public GameObject Get(int key)
		{
			return items.HashDictionary.TryGetValue(key, out var item)
				? item.asset
				: throw new KeyNotFoundException($"Prefab with key '{key}' not found at prefab repository!");
		}

#if UNITY_EDITOR

		[Button]
		private void Fill()
		{
			items = UnityEditor.AssetDatabase.FindAssets("t:Prefab").Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
																	.Select(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>)
																	.OrderBy(s => s.name)
																	.ToList();

			UnityEditor.EditorUtility.SetDirty(this);
		}

#endif
	}
}