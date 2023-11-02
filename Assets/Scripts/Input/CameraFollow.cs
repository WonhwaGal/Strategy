using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float yDistance;
    [SerializeField] private float xShift;
    [SerializeField] private float zShift;

    private Transform _targetT;

    public void AssignTarget(Transform target)
    {
        _targetT = target;
        transform.position = _targetT.position + new Vector3(xShift, yDistance, zShift);
    }

    private void Update()
    {
        if (_targetT == null)
            return;

        float xPos = _targetT.position.x + xShift;
        float zPos = _targetT.position.z + zShift;
        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }
}