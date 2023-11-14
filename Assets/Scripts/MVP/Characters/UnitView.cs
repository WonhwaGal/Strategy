using System;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitView : MonoBehaviour, IUnitView
    {
        [SerializeField] private PrefabType _prefabType;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private HPBar _hpBar;

        public NavMeshAgent NavAgent => _agent;
        public HPBar HPBar => _hpBar;
        public PrefabType PrefabType => _prefabType;
        public GameObject GameObject => gameObject;

        public event Action<float> OnUpdate;
        public event Action<int> OnReceiveDamage;
        public event Action OnViewDestroyed;

        private void Update() => OnUpdate?.Invoke(Time.deltaTime);

        protected void ReceiveDamage(int damage) => OnReceiveDamage?.Invoke(damage);

        private void OnDestroy()  => OnViewDestroyed?.Invoke();
    }
}