using System;
using Code.Pools;
using Code.Units;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ConstructionView : MonoBehaviour, IUnitView, ISpawnableType
{
    [SerializeField] private PrefabType _prefabType;
    [SerializeField] private GameObject[] _previews;
    [SerializeField] private GameObject[] _viewStages;

    private int _currentStage = -1;

    public PrefabType PrefabType => _prefabType;

    public event Action<BuildActionType> OnTriggerAction;
    public event Action<int> OnStageChange;
    public event Action<float> OnUpdate;

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

        OnStageChange?.Invoke(_currentStage += addValue);
        for (int i = 0; i < _viewStages.Length; i++)
            _viewStages[i].SetActive(i == _currentStage);
    }

    private void ShowPreviewPoint(bool toShow)
    {
        for(int i = 0; i < _previews.Length; i++)
        {
            _previews[i].SetActive(false);
            if(i == _currentStage + 1)
                _previews[i].SetActive(toShow);
        }
    }

    private void OnDestroy()
    {
        OnTriggerAction = null;
        OnUpdate = null;
    }
}
