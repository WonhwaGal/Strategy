using System;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitView : MonoBehaviour, IUnitView
    {
        [SerializeField] private float _speed;
        [SerializeField] private NavMeshAgent _agent;

        public NavMeshAgent NavAgent => _agent;
        public float Speed { get => _speed; }

        public event Action OnUpdate;

        private void Update() => OnUpdate?.Invoke();
    }
}