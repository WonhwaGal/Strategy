using UnityEngine;

namespace Code.Factories
{
    public sealed class SingleTypeFactory<T> : Factory<T>
        where T : MonoBehaviour, ISpawnableType
    {
        public SingleTypeFactory(T prefab) : base(prefab) { }

        public override T Create() => Object.Instantiate(_prefab);
    }
}