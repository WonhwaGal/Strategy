using System;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitView : MonoBehaviour, IUnitView
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private HPBar _hpBar;

        public NavMeshAgent NavAgent => _agent;
        public HPBar HPBar => _hpBar;

        public event Action<float> OnUpdate;
        public event Action OnReceiveDamage;

        private void Update() => OnUpdate?.Invoke(Time.deltaTime);

        protected void ReceiveDamage() => OnReceiveDamage?.Invoke();
    }
}