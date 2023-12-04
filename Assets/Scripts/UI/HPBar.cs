using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HPBar : FollowUIView
    {
        [SerializeField] private Slider _hpSlider;

        public HPBar SetUpSlider(int maxValue, Transform owner)
        {
            _hpSlider.maxValue = maxValue;
            _hpSlider.value = maxValue;
            _owner = owner;
            _isSetUp = true;
            gameObject.SetActive(false);
            return this;
        }

        public void SetHPValue(int value) => _hpSlider.value = value;
    }
}