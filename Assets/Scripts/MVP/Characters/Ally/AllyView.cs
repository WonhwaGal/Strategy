using Code.Combat;
using Code.Units;
using System;
using UnityEngine;

public class AllyView : UnitView
{
    public Vector3 OrderedPosition { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ArrowView arrow))
        {
            arrow.ReturnToPool();
            ReceiveDamage(arrow.Damage);
        }
    }
}