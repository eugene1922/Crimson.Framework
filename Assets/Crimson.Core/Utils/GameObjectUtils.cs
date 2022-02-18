using Assets.Crimson.Core.Loading;
using Unity.Entities;
using UnityEngine;

namespace Crimson.Core.Utils
{
	public static class GameObjectUtils
	{
		public static GameObject CreateFromPrefab(this GameObject go)
		{
			return PoolDictionary.Instance.Get(go);
		}

		public static void Destroy(this GameObject go)
		{
			PoolDictionary.Instance.Release(go);
		}

		public static void DestroyWithEntity(this GameObject go, Entity entity)
		{
			go.Destroy();
			World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(entity);
		}

		public static T SafeAddComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
		}
	}
}