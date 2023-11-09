using Code.ScriptableObjects;
using UnityEngine;

namespace Code.Pools
{
    public sealed class ConstructionPool<T> : Pool<T>
        where T : ConstructionView, ISpawnableType
    {
        public ConstructionPool(ConstructionPrefabs prefabs)
        {
            for (int i = 0; i < prefabs.Prefabs.Count; i++)
            {
                var newType = prefabs.Prefabs[i].Type;
                var newFactory = new ConstructionFactory<T>((T)prefabs.Prefabs[i].Prefab);
                _factories.Add(newType, newFactory);
                var rootObject = new GameObject(newType.ToString());
                _factories[newType].RootTransform = rootObject.transform;
            }
        }

        protected override void OnSpawn(T prefab, ISpawnableType spawnData)
        {
            ISpawnBuilding spawnBuilding = (ISpawnBuilding)spawnData;
            prefab.transform.position = spawnBuilding.Transform.position;
            prefab.transform.rotation = spawnBuilding.Transform.rotation;
            var parent = _factories[spawnBuilding.PrefabType].RootTransform;
            prefab.transform.SetParent(parent);
        }

        public override void Dispose()
        {
            _factories.Clear();
            _inactiveObjects.Clear();
        }
    }
}