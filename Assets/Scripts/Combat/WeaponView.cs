using Code.Pools;
using UnityEngine;

namespace Code.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class WeaponView : MonoBehaviour, ISpawnableType
    {
        [SerializeField] protected PrefabType _prefabType;
        [SerializeField] private int _damage;

        protected Rigidbody _rb;

        public PrefabType PrefabType => _prefabType;
        public int Damage => _damage;
    }
}