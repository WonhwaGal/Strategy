using System;
using Code.Units;
using Code.Pools;
using UnityEngine;
using Code.Weapons;

public class CombatService : IService
{
    //private readonly SingleTypePool<ArrowView> _arrowPool;
    private readonly MultiPool<WeaponView> _weaponPool;
    private readonly Collider[] _colliders;
    private LayerMask _enemyMask;
    private LayerMask _playerMask;
    private LayerMask _buildingMask;
    private LayerMask _allyMask;

    public CombatService(WeaponList weaponList)
    {
        _weaponPool = new ObjectMultiPool(weaponList);
        _enemyMask = LayerMask.GetMask("Enemies");
        _playerMask = LayerMask.GetMask("Player");
        _buildingMask = LayerMask.GetMask("Building");
        _allyMask = LayerMask.GetMask("Allies");
        _colliders = new Collider[25];
    }

    public event Action<GameObject> OnLocatingCollider;

    public void CheckForTargets(IModel model, AttackType attack)
    {
        if ((int)model.PrefabType <= (int)PrefabType.Player)
            ScanArea(model.Transform.position, model.Radius, _enemyMask, attack);
        else
            ScanArea(model.Transform.position, model.Radius, _playerMask | _buildingMask | _allyMask, attack);
    }

    private void ScanArea(Vector3 origin, float radius, int mask, AttackType attack)
    {
        int number = Physics.OverlapSphereNonAlloc(origin, radius, _colliders, mask);
        if (number == 0)
            return;

        if (attack == AttackType.Arrow)
            ShootArrow(number, origin);
        else
        {
            for (int i = 0; i < number; i++)
                OnLocatingCollider?.Invoke(_colliders[i].gameObject);
        }
    }

    private void ShootArrow(int numberOfFounds, Vector3 origin)
    {
        var result = FindClosestOpponent(numberOfFounds, origin);
        ArrowView arrow = (ArrowView)_weaponPool.Spawn(PrefabType.Arrow);
        arrow.OnReachTarget += DespawnArrow;
        arrow.AssignTarget(origin, result);
    }

    private Transform FindClosestOpponent(int numberOfFounds, Vector3 origin)
    {
        Collider closest = _colliders[0];
        if (numberOfFounds == 1)
            return closest.transform;

        float dist = (closest.transform.position - origin).sqrMagnitude;
        for (int i = 1; i < numberOfFounds; i++)
        {
            var currentDist = (_colliders[i].transform.position - origin).sqrMagnitude;
            if ((_colliders[i].transform.position - origin).sqrMagnitude > dist)
                continue;
            dist = currentDist;
            closest = _colliders[i];
        }
        return closest.transform;
    }

    private void DespawnArrow(ArrowView arrow)
    {
        _weaponPool.Despawn(arrow);
        arrow.OnReachTarget -= DespawnArrow;
    }
}