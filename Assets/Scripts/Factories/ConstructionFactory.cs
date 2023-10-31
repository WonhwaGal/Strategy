using UnityEngine;

namespace Code.Factories
{
    public sealed class ConstructionFactory<T> : Factory<T>
        where T : ConstructionView
    {
        public ConstructionFactory(T prefab) : base(prefab) { }

        public override T Create() => Object.Instantiate(_prefab);
    }
}