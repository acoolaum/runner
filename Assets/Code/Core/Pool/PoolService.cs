using System;
using System.Collections.Generic;

namespace Acoolaum.Core.Services
{
    public class PoolService : IService
    {
        private Dictionary<PoolableMonoBehaviour, Pool> _pools;
        
        public Pool AddPool(PoolableMonoBehaviour prefab, int amount)
        {
            if (_pools.ContainsKey(prefab)) {
                throw new ArgumentException($"Pool with prefab {prefab.name} already exists");
            }

            var newPool = new Pool(prefab, amount);
            _pools.Add(prefab, newPool);
            return newPool;
        }

        public Pool GetPool(PoolableMonoBehaviour prefab)
        {
            if (_pools.TryGetValue(prefab, out var pool)) {
                return pool;
            }
            
            throw new ArgumentException($"Pool with prefab {prefab.name} not exists");
        }

        public PoolableMonoBehaviour Get(PoolableMonoBehaviour prefab)
        {
            if (_pools.TryGetValue(prefab, out var pool) == false) {
                pool = AddPool(prefab, 1);
            }

            return pool.Get();
        }

        void IService.Initialize(ServiceContainer serviceContainer)
        {
            _pools = new Dictionary<PoolableMonoBehaviour, Pool>();
        }
        
        void IService.Dispose()
        {
            foreach (var pool in _pools) {
                pool.Value.Dispose();
            }
            _pools.Clear();
        }
    }
}