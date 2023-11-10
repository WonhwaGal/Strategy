using UnityEngine;

namespace Code.Pools
{
    public class SingleFactory<T>: Factory<T>
        where T : MonoBehaviour, ISpawnableType
    {
        public SingleFactory(T prefab) : base(prefab) { }

        public override T Create() => GameObject.Instantiate(_prefab);
    }
}