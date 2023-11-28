using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class RechargePanel : FollowUIView
    {
        [SerializeField] private GameObject _rechargeImage;
        [SerializeField] private Image _timerCircle;
        private float _rechargeSpan;
        private bool _isReady;
        private bool _isRunning;

        public bool IsReady
        {
            get => _isReady;
            set
            {
                _isReady = value;
                if (!value)
                    StartCoroutine(CountDown(_rechargeSpan));
            }
        }

        public RechargePanel SetUpPanel(float rechargeSpan, Transform owner)
        {
            _rechargeSpan = rechargeSpan;
            _owner = owner;
            _isSetUp = true;
            if(!_isRunning)
                StartCoroutine(CountDown(_rechargeSpan));
            return this;
        }

        private IEnumerator CountDown(float duration)
        {
            _isRunning = true;
            _rechargeImage.SetActive(true);
            float passedTime = 0;

            while (passedTime < duration)
            {
                passedTime += Time.deltaTime;
                _timerCircle.fillAmount = Mathf.InverseLerp(0f, 1f, passedTime / duration);
                yield return null;
            }

            IsReady = true;
            _isRunning = false;
            _rechargeImage.SetActive(false);
        }

        public void CancelCountDown() => StopAllCoroutines();

        private void OnDestroy() => CancelCountDown();
    }
}