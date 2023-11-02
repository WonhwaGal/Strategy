using Code.Units;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ConstructionView : MonoBehaviour, IUnitView
{
    [SerializeField] private GameObject[] Previews;
    [SerializeField] private GameObject[] ViewStages;
    private int _currentStage = -1;

    public event Action<BuildActionType> OnTriggerAction;
    public event Action OnUpdate;

    private void OnEnable() => ShowCurrentStage();
    private void Update() => OnUpdate?.Invoke();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerUnit>())
            OnTriggerAction?.Invoke(BuildActionType.Show);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerUnit>())
            OnTriggerAction?.Invoke(BuildActionType.PutAway);
    }

    public void Show(BuildActionType action)
    {
        if (action == BuildActionType.Build)
            ShowCurrentStage(1);
        else
            ShowPreviewPoint(action == BuildActionType.Show);
    }

    public void ShowCurrentStage(int addValue = 0)
    {
        ShowPreviewPoint(false);
        if (_currentStage >= 0)
            ViewStages[_currentStage].SetActive(false);

        _currentStage += addValue;
        for (int i = 0; i < ViewStages.Length; i++)
            ViewStages[i].SetActive(i == _currentStage);
    }

    private void ShowPreviewPoint(bool toShow)
    {
        for(int i = 0; i < Previews.Length; i++)
        {
            Previews[i].SetActive(false);
            if(i == _currentStage + 1)
                Previews[i].SetActive(toShow);
        }
    }

    private void OnDestroy()
    {
        OnTriggerAction = null;
        OnUpdate = null;
    }
}
