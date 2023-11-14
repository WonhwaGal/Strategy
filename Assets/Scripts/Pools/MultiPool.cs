using System.Collections.Generic;
using UnityEngine;

namespace Code.Pools
{
    public abstract class MultiPool<K, T> : IService
        where T : MonoBehaviour, ISpawnableType
    {
        private readonly Dictionary<K, SinglePool> _pools = new();

        public T Spawn(K type)
        {
            if (!_pools.TryGetValue(type, out SinglePool pool))
            {
                pool = new SinglePool(GetPrefab(type));
                _pools.Add(type, pool);
            }

            return pool.Spawn();
        }

        public void Despawn(K key,T prefab)
        {
            if (_pools.ContainsKey(key))
                _pools[key].Despawn(prefab);
            else
                GameObject.Destroy(prefab);
        }

        protected abstract T GetPrefab(K type);
        public virtual void OnSpawned(T result, ISpawnableType data) { }

        private sealed class SinglePool : BaseSinglePool<T>
        {
            public SinglePool(T prefab) : base(prefab) { }
        }
    }
}