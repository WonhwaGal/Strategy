using System;
using Code.Units;
using UnityEngine;

namespace Code.Construction
{
    [RequireComponent(typeof(Rigidbody))]
    public class ConstructionView : MonoBehaviour, IUnitView
    {
        [SerializeField] private PrefabType _prefabType;
        [SerializeField] private GameObject[] _previews;
        [SerializeField] private GameObject[] _viewStages;
        [SerializeField] private GameObject _brokenView;
        [SerializeField] private HPBar _hpBar;

        private int _currentStage = -1;

        public PrefabType PrefabType => _prefabType;
        public GameObject GameObject => gameObject;
        public HPBar HPBar => _hpBar;
        public GameObject BrokenView => _brokenView;

        public event Action<BuildActionType> OnTriggerAction;
        public event Action<int> OnModeChange;
        public event Action<float> OnUpdate;
        public event Action OnViewDestroyed;

        private void OnEnable() => ShowCurrentStage();
        private void Update() => OnUpdate?.Invoke(Time.deltaTime);

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerUnit>() && _currentStage >= 0)
                OnTriggerAction?.Invoke(BuildActionType.Show);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerUnit>() && _currentStage >= 0)
                OnTriggerAction?.Invoke(BuildActionType.PutAway);
        }

        public void React(BuildActionType action)
        {
            if (action == BuildActionType.Build || action == BuildActionType.Upgrade)
                ShowCurrentStage(1);
            else
                ShowPreviewPoint(action == BuildActionType.Show);
        }

        public void ShowCurrentStage(int addValue = 0)
        {
            ShowPreviewPoint(false);
            if (_currentStage >= 0)
                _viewStages[_currentStage].SetActive(false);

            OnModeChange?.Invoke(_currentStage += addValue);
            for (int i = 0; i < _viewStages.Length; i++)
                _viewStages[i].SetActive(i == _currentStage);
        }

        private void ShowPreviewPoint(bool toShow)
        {
            for (int i = 0; i < _previews.Length; i++)
            {
                _previews[i].SetActive(false);
                if (i == _currentStage + 1)
                    _previews[i].SetActive(toShow);
            }
            _brokenView.SetActive(false);
            _hpBar.gameObject.SetActive(false);
        }

        private void OnDestroy() => OnViewDestroyed?.Invoke();
    }
}