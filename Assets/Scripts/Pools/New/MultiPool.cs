using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Pools
{
    public abstract class MultiPool<T> : IService
        where T : MonoBehaviour, ISpawnableType
    {
        private Dictionary<Enum, SinglePool> _pools = new();

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

        private sealed class SinglePool : BaseSinglePool<T>
        {
            public SinglePool(T prefab) : base(prefab) { }
        }
    }

    internal abstract class BaseSinglePool<T> where T : MonoBehaviour, ISpawnableType
    {
        protected readonly Stack<T> _inactives = new();
        protected readonly Factory<T> _factory;

        public BaseSinglePool(T prefab) => _factory = new SingleFactory<T>(prefab);

        public T Spawn()
        {
            T result;
            if (_inactives.Count > 0)
                result = _inactives.Pop();
            else
                result = _factory.Create();

            OnSpawn(result);
            return result;
        }

        public void Despawn(T prefab)
        {
            prefab.gameObject.SetActive(false);
            _inactives.Push(prefab);

            OnDespawn(prefab);
        }

        protected virtual void OnSpawn(T prefab) { }
        protected virtual void OnDespawn(T prefab) { }

        public void Dispose() => _inactives.Clear();
    }

    public class SingleFactory<T>: Factory<T>
        where T : MonoBehaviour, ISpawnableType
    {
        public SingleFactory(T prefab) : base(prefab) { }

        public override T Create() => GameObject.Instantiate(_prefab);
    }
}