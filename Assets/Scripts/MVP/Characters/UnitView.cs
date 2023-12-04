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
        [SerializeField] private Animator _animator;
        [SerializeField] private UIType _hpBarType;

        public NavMeshAgent NavAgent => _agent;
        public Animator Animator => _animator;
        public PrefabType PrefabType => _prefabType;
        public GameObject GameObject => gameObject;
        public bool HasPresenter { get; set; }
        public UIType HPBarType => _hpBarType;

        public event Action<float> OnUpdate;
        public event Action<int> OnReceiveDamage;
        public event Action<bool> OnViewDestroyed;

        private void Update() => OnUpdate?.Invoke(Time.deltaTime);

        protected void ReceiveDamage(int damage) => OnReceiveDamage?.Invoke(damage);

        private void OnDestroy()  => OnViewDestroyed?.Invoke(true);
    }
}