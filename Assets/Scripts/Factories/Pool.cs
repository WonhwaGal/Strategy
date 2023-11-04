using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Factories
{
    public abstract class Pool<T> : IDisposable
        where T : MonoBehaviour
    {
        protected Dictionary<Enum, Stack<T>> _inactiveObjects = new();
        protected Dictionary<Enum, Factory<T>> _factories = new();

        public T Spawn(ISpawnable spawnData)
        {
            T result = null;
            PrefabType type = spawnData.CommonInfo.PrefabType;

            if (_inactiveObjects.ContainsKey(type) && _inactiveObjects[type].Count > 0)
                result = _inactiveObjects[spawnData.CommonInfo.PrefabType].Pop();
            else if(_factories.ContainsKey(type))
                result = _factories[type].Create();

            OnSpawn(result, spawnData);

            return result;
        }

        public void Despawn() { }

        protected virtual void OnSpawn(T prefab, ISpawnable spawnObj) { }
        protected virtual void OnDespawn() { }

        public virtual void Dispose() { }
    }
}