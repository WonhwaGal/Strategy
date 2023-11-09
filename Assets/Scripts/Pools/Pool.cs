using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Pools
{
    public abstract class Pool<T> : IDisposable
        where T : MonoBehaviour, ISpawnableType
    {
        protected Dictionary<Enum, Stack<T>> _inactiveObjects = new();
        protected Dictionary<Enum, Factory<T>> _factories = new();

        public T Spawn(ISpawnableType spawnData)
        {
            T result = null;
            PrefabType type = spawnData.PrefabType;

            if (_inactiveObjects.ContainsKey(type) && _inactiveObjects[type].Count > 0)
                result = _inactiveObjects[spawnData.PrefabType].Pop();
            else if (_factories.ContainsKey(type))
                result = _factories[type].Create();

            OnSpawn(result, spawnData);
            return result;
        }

        public void Despawn(T prefab)
        {
            prefab.gameObject.SetActive(false);

            if (!_inactiveObjects.ContainsKey(prefab.PrefabType))
                _inactiveObjects.Add(prefab.PrefabType, new Stack<T>());
            _inactiveObjects[prefab.PrefabType].Push(prefab);

            OnDespawn(prefab);
        }

        protected virtual void OnSpawn(T prefab, ISpawnableType spawnData = null) { }
        protected virtual void OnDespawn(T prefab) { }

        public abstract void Dispose();
    }
}