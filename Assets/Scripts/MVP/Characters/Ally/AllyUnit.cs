using System;
using UnityEngine;
using Code.Combat;

namespace Code.Units
{
    public class AllyUnit : UnitView
    {
        [SerializeField] private GameObject _staticCircle;
        private bool _underControl;
        private bool _isStatic;

        public bool IsStaticInCombat
        {
            get => _isStatic;
            set
            {
                if(value && !_isStatic)
                    _staticCircle.SetActive(true);
                _isStatic = value;
            }
        }
        public bool UnderPlayerControl
        {
            get => _underControl;
            set
            {
                if(_underControl && !value)
                {
                    Leader = null;
                    OrderedPosition = transform.position;
                }
                else if(!_underControl && value)
                {
                    OnGetUnderControl?.Invoke(Leader);
                    _staticCircle.SetActive(false);
                }

                _underControl = value;
            }
        }
        public Vector3 OrderedPosition { get; set; }
        public Transform Leader { get; private set; }


        public event Action<Transform> OnGetUnderControl;

        private void Start() => _staticCircle.SetActive(false);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArrowView arrow))
            {
                arrow.ReturnToPool();
                ReceiveDamage(arrow.Damage);
            }
            else if (!UnderPlayerControl && other.GetComponentInParent<PlayerUnit>())
            {
                Leader = other.transform;
                UnderPlayerControl = true;
            }
        }
    }
}