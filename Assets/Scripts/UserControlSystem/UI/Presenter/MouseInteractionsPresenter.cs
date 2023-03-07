using System.Linq;
using UnityEngine;
using UserControlSystem;
using Abstractions;
using UnityEngine.EventSystems;

public sealed class MouseInteractionsPresenter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectableValue _selectedObject;
    [SerializeField] private EventSystem _eventSystem;


    private ISelectable _lastSelectable;
    private void Update()
    {
        if (!Input.GetMouseButtonUp(0) || _eventSystem.IsPointerOverGameObject())
        {
            return;
        }
        var hits = Physics.RaycastAll(_camera.ScreenPointToRay(Input.mousePosition));
        if (hits.Length == 0)
        {
            return;
        }
        var selectable = hits
                             .Select(hit => hit.collider.GetComponentInParent<ISelectable>())
                             .Where(c => c != null)
                             .FirstOrDefault();

        CancelPreviousSelection(selectable);

        _selectedObject.SetValue(selectable);
        _lastSelectable = selectable;
    }

    private void CancelPreviousSelection(ISelectable sel)
    {
        if (sel != null)
            sel.UpdateSelection(true);

        if (_lastSelectable != null && sel != _lastSelectable) 
            _lastSelectable.UpdateSelection(false);
    }
}
