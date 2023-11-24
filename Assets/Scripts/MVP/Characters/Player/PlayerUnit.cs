using Code.Units;
using UnityEngine;

public class PlayerUnit : UnitView
{
    [SerializeField] private GameObject _controlUnitArea;

    public GameObject ControlUnitArea => _controlUnitArea;

    private void Start()
    {
        _controlUnitArea.SetActive(false);
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        camFollow.AssignTarget(transform);
    }
}