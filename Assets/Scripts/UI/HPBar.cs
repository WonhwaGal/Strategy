using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HPBar : UIView
    {
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private float _verticalShift;

        private Transform _owner;
        private Camera _camera;
        private bool _isSetUp;

        public event Action<HPBar> OnOwnerKilled;

        private void OnEnable() => _camera = Camera.main;

        public HPBar SetUpSlider(int maxValue, Transform owner)
        {
            _hpSlider.maxValue = maxValue;
            _hpSlider.value = maxValue;
            _owner = owner;
            _isSetUp = true;
            gameObject.SetActive(false);
            return this;
        }

        private void LateUpdate()
        {
            if (!_isSetUp)
                return;

            FollowOwner();
        }

        public void SetHPValue(int value) => _hpSlider.value = value;

        public void Despawn() => OnOwnerKilled?.Invoke(this);

        private void FollowOwner()
        {
            _hpSlider.transform.position = _camera.WorldToScreenPoint(_owner.position);
            _hpSlider.transform.position += new Vector3(0, _verticalShift, 0);
        }
    }
}