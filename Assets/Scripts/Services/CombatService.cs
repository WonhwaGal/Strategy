using Code.Units;
using System;
using UnityEngine;

public class CombatService : IService
{
    private LayerMask _enemyMask;
    private LayerMask _playerMask;
    private LayerMask _buildingMask;
    private LayerMask _allyMask;

    public CombatService()
    {
        _enemyMask = LayerMask.GetMask("Enemies");
        _playerMask = LayerMask.GetMask("Player");
        _buildingMask = LayerMask.GetMask("Building");
        _allyMask = LayerMask.GetMask("Allies");
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
        Collider[] colliders = Physics.OverlapSphere(origin, radius, mask);
        if (colliders.Length == 0)
            return;

        if (attack == AttackType.Arrow)
        {
            var result = FindClosestOpponent(colliders, origin);
            Debug.Log("found enemy" + result.name);
        }
        else
        {
            Debug.Log("send scanned area event");
            for (int i = 0; i < colliders.Length; i++)
                OnLocatingCollider?.Invoke(colliders[i].gameObject);
        }
    }

    private Transform FindClosestOpponent(Collider[] list, Vector3 origin)
    {
        if (list.Length == 1)
            return list[0].transform;

        Collider closest = list[0];
        float dist = (closest.transform.position - origin).sqrMagnitude;
        for (int i = 1; i < list.Length; i++)
        {
            var currentDist = (list[i].transform.position - origin).sqrMagnitude;
            if ((list[i].transform.position - origin).sqrMagnitude > dist)
                continue;
            dist = currentDist;
            closest = list[i];
        }
        return closest.transform;
    }
}