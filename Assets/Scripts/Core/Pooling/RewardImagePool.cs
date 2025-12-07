using System.Collections.Generic;
using Core.Services;
using UI;
using UnityEngine;

namespace Core.Pooling
{
    public class RewardImagePool : MonoBehaviour, IRewardImagePool
    {
        [SerializeField] private RewardImage rewardImagePrefab;

        private ObjectPool<RewardImage> _rewardImagePool;

        private const int InitialSize = 20;

        private readonly Dictionary<string, object> _pools = new();

        private void Awake()
        {
            Initialize();
            RegisterService();
        }

        private void Initialize()
        {
            if (rewardImagePrefab == null)
            {
                Debug.LogError($"[rewardImagePrefab] Prefab is null for pool creation!");
                return;
            }

            _rewardImagePool = CreatePool(rewardImagePrefab, InitialSize);
            _rewardImagePool.SetSpawnCallback(image =>
            {
                image.GetComponent<RewardImage>()?.SetPool(_rewardImagePool);
            });
        }

        private void RegisterService()
        {
            ServiceLocator.Register<IRewardImagePool>(this);
        }

        private ObjectPool<T> CreatePool<T>(T prefab, int initialSize = 10, bool autoExpand = true)
            where T : Component
        {
            var key = GetPoolKey<T>(prefab.name);

            if (_pools.ContainsKey(key))
            {
                Debug.LogWarning($"Pool for {prefab.name} already exists!");
                return _pools[key] as ObjectPool<T>;
            }

            var pool = new ObjectPool<T>(prefab, initialSize, autoExpand, transform);
            _pools.Add(key, pool);

            return pool;
        }

        public RewardImage GetRewardImage(Vector3 position)
        {
            return _rewardImagePool.Get(position);
        }

        public ObjectPool<T> GetPool<T>(string prefabName) where T : Component
        {
            var key = GetPoolKey<T>(prefabName);

            if (_pools.TryGetValue(key, out object pool))
            {
                return pool as ObjectPool<T>;
            }

            Debug.LogWarning($"Pool for {prefabName} not found!");
            return null;
        }

        private string GetPoolKey<T>(string prefabName) => $"{typeof(T).Name}_{prefabName}";
    }
}