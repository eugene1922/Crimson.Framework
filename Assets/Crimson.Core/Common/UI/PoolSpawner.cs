using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Crimson.Core.Common.UI
{
    [Serializable]
    public class PoolSpawner<T> where T : MonoBehaviour
    {
        private readonly List<T> _instances = new List<T>();
        [SerializeField] private int _poolSize = 10;
        [SerializeField] private T _prefab;
        [SerializeField] private Transform _root;

        public T Get()
        {
            var instance = _instances.Find(s => !s.gameObject.activeSelf);
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void Init()
        {
            for (var i = 0; i < _poolSize; i++)
            {
                var instance = SpawnEmpty();
                Release(instance);
            }
        }

        public void Release(T instance)
        {
            instance.gameObject.SetActive(false);
        }

        public void ReleaseAll()
        {
            for (var i = 0; i < _instances.Count; i++)
            {
                Release(_instances[i]);
            }
        }

        private T SpawnEmpty()
        {
            var instance = UnityEngine.Object.Instantiate(_prefab, _root);
            _instances.Add(instance);
            return instance;
        }
    }
}