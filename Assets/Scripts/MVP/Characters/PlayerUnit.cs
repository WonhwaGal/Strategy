using Code.Units;
using UnityEngine;

public class PlayerUnit : UnitView
{
    private void Start()
    {
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        camFollow.AssignTarget(transform);
    }
}