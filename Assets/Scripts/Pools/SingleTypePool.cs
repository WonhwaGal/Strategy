using System.Collections.Generic;
using UnityEngine;

namespace Code.Factories
{
    public class SingleTypePool<T> : Pool<T>
        where T : MonoBehaviour, ISpawnableType
    {
        private readonly T _prefab;

        public SingleTypePool(T prefab)
        {
            _prefab = prefab;
            var newFactory = new SingleTypeFactory<T>(_prefab);
            _factories.Add(_prefab.PrefabType, newFactory);
            GameObject rootObject = new(_prefab.GetType().ToString());
            newFactory.RootTransform = rootObject.transform;
        }

        protected override void OnSpawn(T prefab, ISpawnableType spawnData)
        {
            prefab.transform.SetParent(_factories[prefab.PrefabType].RootTransform);
        }

        public override void Dispose()
        {
            _factories.Clear();
            _inactiveObjects.Clear();
        }
    }
}