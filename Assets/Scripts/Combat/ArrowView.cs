using System;
using UnityEngine;
using Code.Pools;

[RequireComponent(typeof(Rigidbody))]
public class ArrowView : MonoBehaviour, ISpawnableType
{
    [SerializeField] private float _speed;
    private Rigidbody _rb;
    private Transform _target;

    public event Action<ArrowView> OnReachTarget;

    public PrefabType PrefabType => PrefabType.Arrow;

    private void Start() => _rb = GetComponent<Rigidbody>();

    private void Update()
    {
        if (_target == null) // add condition for flying further
            return;

        MoveInTrajectory();
    }

    public void MoveInTrajectory()
    {
        var dir = _target.position - transform.position;
        _rb.velocity = dir * _speed;
    }

    public void AssignTarget(Vector3 origin, Transform target)
    {
        transform.position = origin;
        var dir = target.position - origin;
        transform.rotation = Quaternion.LookRotation(dir);
        _target = target;
    }

    public void ReturnToPool() => OnReachTarget?.Invoke(this);

    private void OnDisable() => _target = null;
}