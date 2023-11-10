using Code.Pools;
using UnityEngine;

namespace Code.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class WeaponView : MonoBehaviour, ISpawnableType
    {
        [SerializeField] protected PrefabType _prefabType;
        protected Rigidbody _rb;

        public PrefabType PrefabType => _prefabType;
    }
}