using System;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitView : MonoBehaviour, IUnitView
    {
        [SerializeField] private NavMeshAgent _agent;

        public NavMeshAgent NavAgent => _agent;

        public event Action<float> OnUpdate;

        private void Update() => OnUpdate?.Invoke(Time.deltaTime);
    }
}