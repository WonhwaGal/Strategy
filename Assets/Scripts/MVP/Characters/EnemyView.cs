using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Units
{
    public class EnemyView : UnitView
    {
        [SerializeField] private Slider _slider;

        public float MaxSliderValue { get => _slider.maxValue; set => _slider.maxValue = value; }

        public event Action OnReceiveDamage;

        private void OnEnable() => _slider.value = MaxSliderValue;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArrowView arrow))
            {
                arrow.ReturnToPool();
                OnReceiveDamage?.Invoke();
            }
        }

        public void ChangeHPSlider(int currentValue)
        {
            _slider.value = currentValue;
        }
    }
}