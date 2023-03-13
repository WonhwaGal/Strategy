using Zenject;
using System.Linq;
using UnityEngine;
using UserControlSystem;
using Abstractions;
using UnityEngine.EventSystems;

public sealed class MouseInteractionsPresenter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [Inject] private SelectableValue _selectedObject;
    [Inject] private AttackableValue _attackedObject;
    [SerializeField] private EventSystem _eventSystem;

    [Inject] private Vector3Value _groundClicksRMB;
    [SerializeField] private Transform _groundTransform;

    private Plane _groundPlane;


    private void Start()
    {
        _groundPlane = new Plane(_groundTransform.up, 0);
    }


    private void Update()
    {
        if (!Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
        {
            return;
        }
        if (_eventSystem.IsPointerOverGameObject())
        {
            return;
        }

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
        {
            var hits = Physics.RaycastAll(ray);
            if (hits.Length == 0)
            {
                return;
            }

            var selectable = hits
                                 .Select(hit => hit.collider.GetComponentInParent<ISelectable>())
                                 .Where(c => c != null)
                                 .FirstOrDefault();

            _selectedObject.SetValue(selectable);
        }
        else
        {
            var hits = Physics.RaycastAll(ray);
            if (hits.Length == 0)
            {
                return;
            }
            var attackable = hits
                                 .Select(hit => hit.collider.GetComponentInParent<IAttackable>())
                                 .Where(c => c != null)
                                 .FirstOrDefault();

            _attackedObject.SetValue(attackable);

            if (_groundPlane.Raycast(ray, out var enter))
            {
                _groundClicksRMB.SetValue(ray.origin + ray.direction * enter);
                _selectedObject.SetValue(null);   // optional - maybe to delete
            }
        }
    }
}
