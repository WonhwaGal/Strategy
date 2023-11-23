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

        public NavMeshAgent NavAgent => _agent;
        public PrefabType PrefabType => _prefabType;
        public GameObject GameObject => gameObject;
        public bool HasPresenter { get; set; }

        public event Action<float> OnUpdate;
        public event Action<int> OnReceiveDamage;
        public event Action<bool> OnViewDestroyed;

        private void Update() => OnUpdate?.Invoke(Time.deltaTime);

        protected void ReceiveDamage(int damage) => OnReceiveDamage?.Invoke(damage);

        private void OnDestroy()  => OnViewDestroyed?.Invoke(true);
    }
}