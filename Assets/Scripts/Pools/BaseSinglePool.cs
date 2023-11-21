using System.Collections.Generic;
using UnityEngine;

namespace Code.Pools
{
    internal abstract class BaseSinglePool<T> where T : MonoBehaviour, ISpawnableType
    {
        protected readonly Stack<T> _inactives = new();
        protected readonly Factory<T> _factory;

        public BaseSinglePool(T prefab)
        {
            _factory = new SingleFactory<T>(prefab);
            if (prefab.PrefabType != PrefabType.None && prefab.PrefabType != PrefabType.UI)
            {
                var root = new GameObject(prefab.PrefabType.ToString());
                _factory.RootTransform = root.transform;
            }
        }

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
            if (prefab == null)
                return;
            prefab.gameObject.SetActive(false);
            _inactives.Push(prefab);
        }

        private void OnSpawn(T prefab)
        {
            if (_factory.RootTransform != null)
                prefab.transform.SetParent(_factory.RootTransform);
            prefab.gameObject.SetActive(true);
        }

        public void Dispose() => _inactives.Clear();
    }
}