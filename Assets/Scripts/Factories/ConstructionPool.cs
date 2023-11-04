using Code.ScriptableObjects;
using UnityEngine;

namespace Code.Factories
{
    public sealed class ConstructionPool<T> : Pool<T>
        where T : ConstructionView
    {
        public ConstructionPool(ConstructionPrefabs prefabs)
        {
            for (int i = 0; i < prefabs.Prefabs.Count; i++)
            {
                var newFactory = new ConstructionFactory<T>((T)prefabs.Prefabs[i].Prefab);
                _factories.Add(prefabs.Prefabs[i].Type, newFactory);
                GameObject rootObject = new(prefabs.Prefabs[i].Type.ToString());
                _factories[prefabs.Prefabs[i].Type].RootTransform = rootObject.transform;
            }
        }

        protected override void OnSpawn(T prefab, ISpawnable spawnData)
        {
            prefab.transform.position = spawnData.Transform.position;
            prefab.transform.rotation = spawnData.Transform.rotation;
            prefab.transform.SetParent(_factories[spawnData.CommonInfo.PrefabType].RootTransform);
        }

        protected override void OnDespawn() { }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}