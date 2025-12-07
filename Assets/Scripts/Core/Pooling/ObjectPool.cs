using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Pooling
{
    public class ObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private Queue<T> _pool;
        private readonly int _initialSize;
        private readonly bool _autoExpand;
        private readonly bool _autoActive;
        private Action<T> _onObjectSpawn;
        private Action<T> _onObjectDespawn;

        public ObjectPool(T prefab, int initialSize = 10, bool autoExpand = true, Transform parent = null,bool autoActive=true)
        {
            _prefab = prefab;
            _initialSize = initialSize;
            _autoExpand = autoExpand;
            _parent = parent;
            _autoActive = autoActive;

            Initialize();
        }

        private void Initialize()
        {
            _pool = new Queue<T>();

            for (var i = 0; i < _initialSize; i++)
            {
                CreateNewInstance();
            }
        }

        private void CreateNewInstance()
        {
            T obj = UnityEngine.Object.Instantiate(_prefab, _parent);
            if (_autoActive)
                obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }

        public T Get(Vector3 position = default, Quaternion rotation = default)
        {
            if (_pool.Count == 0 && _autoExpand)
            {
                CreateNewInstance();
            }

            if (_pool.Count > 0)
            {
                T obj = _pool.Dequeue();
                obj.transform.position = position;
                //obj.transform.rotation = rotation;
                obj.gameObject.SetActive(true);

                _onObjectSpawn?.Invoke(obj);
                return obj;
            }

            Debug.LogWarning($"Pool for {typeof(T)} is empty and autoExpand is false!");
            return null;
        }

        public void Release(T obj)
        {
            if (!obj.gameObject.activeSelf) return;
            
            if(_autoActive)
                obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
            _onObjectDespawn?.Invoke(obj);
        }

        public void SetSpawnCallback(Action<T> callback)
        {
            _onObjectSpawn = callback;
        }

        public void SetDespawnCallback(Action<T> callback)
        {
            _onObjectDespawn = callback;
        }
    }
}
