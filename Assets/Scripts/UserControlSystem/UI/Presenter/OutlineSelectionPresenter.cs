using Zenject;
using UnityEngine;
using Abstractions;
using UserControlSystem;


public class OutlineSelectionPresenter : MonoBehaviour
{
    [Inject] private SelectableValue selectableValue;

    private ISelectable _lastSelectable;

    private void Awake()
    {
        selectableValue.OnSelected += OnSelected;
    }

    private void OnSelected(ISelectable selectable)
    {
        UpdateCurrentSelection(selectable);
        _lastSelectable = selectable;
    }

    private void UpdateCurrentSelection(ISelectable selectable)
    {
        if (selectable != null)
            selectable.UpdateSelection(true);

        if (_lastSelectable != null && selectable != _lastSelectable)
            _lastSelectable.UpdateSelection(false);
    }
}
