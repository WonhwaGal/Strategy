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
        private float _totalLength;

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
            if (_totalLength != 0 && (!_target.gameObject.activeSelf || _target == null))
                ReturnToPool();
        }

        public void MoveInTrajectory()
        {
            float currentProgress = (transform.position - _origin).magnitude / _totalLength;
            currentProgress *= _parabolaWidthMultiplier;
            float yAdd = (Mathf.Pow(currentProgress, 2) - _height) * -1;

            var newDir = (_lastTargetPos - transform.position).normalized + new Vector3(0, yAdd, 0);
            _rb.velocity = newDir.normalized * _speed;

            var newRot = Quaternion.LookRotation(newDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, currentProgress);
        }

        public void AssignTarget(Vector3 origin, Transform target)
        {
            _target = target;
            _lastTargetPos = _target.position;
            _origin = origin;
            transform.position = _origin;
            var dir = target.position - origin;
            transform.rotation = Quaternion.LookRotation(dir);

            _parabolaWidthMultiplier = Mathf.Sqrt(_height);
            _totalLength = (target.position - origin).magnitude;
        }

        public void ReturnToPool() => OnReachTarget?.Invoke(this);

        private void OnDisable()
        {
            _target = null;
            _lastTargetPos = Vector3.zero;
            _origin = Vector3.zero;
            _parabolaWidthMultiplier = 0;
            _totalLength = 0;
        }
    }
}