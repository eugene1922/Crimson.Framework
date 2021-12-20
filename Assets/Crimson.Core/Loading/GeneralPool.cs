using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Crimson.Core.Loading
{
    public class GeneralPool
    {
        private ObjectPool<GameObject> _pool;

        public GeneralPool(GameObject sourcePrefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            SourcePrefab = sourcePrefab;
            _pool = new ObjectPool<GameObject>(CreateItem,
                                               OnTakeFromPool,
                                               OnReturnedToPool,
                                               OnDestroyObject,
                                               collectionCheck,
                                               defaultCapacity,
                                               maxSize);
        }

        public GeneralPool(GameObject sourcePrefab, ObjectPoolSettings settings) : this(sourcePrefab, settings.CollectionCheck, settings.InitSize, settings.MaxSize)
        {
        }

        public GameObject SourcePrefab { get; }

        public GameObject Get()
        {
            return _pool.Get();
        }

        public void Release(GameObject element)
        {
            _pool.Release(element);
        }

        private GameObject CreateItem()
        {
            var prefab = SourcePrefab;
            return Object.Instantiate(prefab);
        }

        private void OnDestroyObject(GameObject item)
        {
            Object.Destroy(item);
        }

        private void OnReturnedToPool(GameObject item)
        {
            item.SetActive(false);
        }

        private void OnTakeFromPool(GameObject item)
        {
            item.SetActive(true);
        }
    }
}