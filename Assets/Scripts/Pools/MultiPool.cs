using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Pools
{
    public abstract class MultiPool<T> : IService
        where T : MonoBehaviour, ISpawnableType
    {
        private readonly Dictionary<Enum, SinglePool> _pools = new();

        public T Spawn(PrefabType type)
        {
            if (!_pools.TryGetValue(type, out SinglePool pool))
            {
                pool = new SinglePool(GetPrefab(type));
                _pools.Add(type, pool);
            }

            return pool.Spawn();
        }

        public void Despawn(T prefab)
        {
            if (_pools.ContainsKey(prefab.PrefabType))
                _pools[prefab.PrefabType].Despawn(prefab);
            else
                GameObject.Destroy(prefab);
        }

        protected abstract T GetPrefab(PrefabType type);
        public virtual void OnSpawned(T result, ISpawnableType data) { }

        private sealed class SinglePool : BaseSinglePool<T>
        {
            public SinglePool(T prefab) : base(prefab) { }
        }
    }
}