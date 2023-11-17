using System;
using UnityEngine;
using Code.Pools;

namespace Code.Combat
{
    public class ArrowView : WeaponView
    {
        [SerializeField] private float _speed;

        [SerializeField, Range(0.1f, 2)] private float _height = 1.5f;

        private Transform _target;
        private Vector3 _lastTargetPos;
        private Vector3 _origin;
        private float _parabolaWidthMultiplier;
        private float _totalDist;
        private float _currentDist;

        public event Action<ArrowView> OnReachTarget;

        private void Start()
        {
            _prefabType = PrefabType.Arrow;
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_target == null || !_target.gameObject.activeSelf)
            {
                if (_lastTargetPos == Vector3.zero)
                    return;
                MoveInTrajectory();
            }
            else
            {
                _lastTargetPos = _target.position;
                MoveInTrajectory();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_totalDist != 0 && (!_target.gameObject.activeSelf || _target == null))
                ReturnToPool();
        }

        public void MoveInTrajectory()
        {
            var newDir = GetDirection();
            _rb.velocity = newDir.normalized * _speed;

            var newRot = Quaternion.LookRotation(newDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, _currentDist);
        }

        public void AssignTarget(Vector3 origin, Transform target)
        {
            _target = target;
            _lastTargetPos = _target.position;
            _origin = origin;
            transform.position = _origin;

            _parabolaWidthMultiplier = Mathf.Sqrt(_height);
            _totalDist = (target.position - origin).magnitude;
            transform.rotation = Quaternion.LookRotation(GetDirection());
        }

        private Vector3 GetDirection()
        {
            _currentDist = (transform.position - _origin).magnitude / _totalDist;
            _currentDist *= _parabolaWidthMultiplier;
            float yAdd = (Mathf.Pow(_currentDist, 2) - _height) * -1;

            return (_lastTargetPos - transform.position).normalized + new Vector3(0, yAdd, 0);
        }

        public void ReturnToPool() => OnReachTarget?.Invoke(this);

        private void OnDisable()
        {
            _target = null;
            _lastTargetPos = Vector3.zero;
            _origin = Vector3.zero;
            _parabolaWidthMultiplier = 0;
            _totalDist = 0;
        }
    }
}