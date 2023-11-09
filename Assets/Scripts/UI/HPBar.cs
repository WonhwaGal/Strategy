using UnityEngine;
using UnityEngine.UI;

namespace Code.Units
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private Slider _hpSlider;

        public float MaxSliderValue { get => _hpSlider.maxValue; set => _hpSlider.maxValue = value; }

        private void OnEnable() => _hpSlider.value = MaxSliderValue;

        public void ChangeHPSlider(int currentValue) => _hpSlider.value = currentValue;
    }
}