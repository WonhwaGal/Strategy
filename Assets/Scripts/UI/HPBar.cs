using UnityEngine;
using UnityEngine.UI;

namespace Code.Units
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private Slider _hpSlider;

        public float MaxSliderValue => _hpSlider.maxValue;

        private void OnEnable() => _hpSlider.value = MaxSliderValue;

        public void ChangeHPSlider(int currentValue) => _hpSlider.value = currentValue;

        public void SetMaxValue(float value) => _hpSlider.maxValue = value;
    }
}