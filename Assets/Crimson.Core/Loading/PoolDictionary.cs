using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Crimson.Core.Loading
{
	[Serializable]
	public class PoolDictionary
	{
		private static PoolDictionary _instance;
		private readonly Dictionary<string, GeneralPool> _pools = new Dictionary<string, GeneralPool>();

		public static PoolDictionary Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new PoolDictionary();
				}
				return _instance;
			}
		}

		public GameObject Get(GameObject prefab)
		{
			return HasPoolFor(prefab) ?
				_pools[ConstructKey(prefab)].Get()
				: UnityEngine.Object.Instantiate(prefab);
		}

		public void Register(GameObject prefab, ObjectPoolSettings settings)
		{
			var key = ConstructKey(prefab);
			if (_pools.ContainsKey(key))
			{
				return;
			}
			_pools.Add(key, new GeneralPool(prefab, settings));
		}

		public void Release(GameObject instance)
		{
			if (HasPoolFor(instance))
			{
				_pools[ConstructKey(instance)].Release(instance);
			}
			else
			{
				UnityEngine.Object.Destroy(instance);
			}
		}

		private string ConstructKey(GameObject item)
		{
			return item.name;
		}

		private bool HasPoolFor(GameObject item)
		{
			return _pools.ContainsKey(ConstructKey(item));
		}
	}
}