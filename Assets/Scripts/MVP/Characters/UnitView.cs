using System;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private NavMeshAgent _agent;

        public NavMeshAgent NavAgent { get => _agent; }
        public float Speed { get => _speed; }

        public event Action OnUpdate;

        private void Start() => _agent = GetComponent<NavMeshAgent>();

        private void Update() => OnUpdate?.Invoke();
    }
}