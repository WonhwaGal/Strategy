using Code.Units;
using UnityEngine;

public class PlayerUnit : UnitView
{
    [SerializeField] private GameObject _controlUnitArea;
    [SerializeField] private GameObject _areaAttack;

    public GameObject ControlUnitArea => _controlUnitArea;
    public GameObject AreaAttack => _areaAttack;

    private void OnEnable() => _areaAttack.SetActive(false);

    private void Start()
    {
        _controlUnitArea.SetActive(false);
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        camFollow.AssignTarget(transform);
    }
}