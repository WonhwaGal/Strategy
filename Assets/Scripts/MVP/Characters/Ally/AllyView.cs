using Code.Combat;
using Code.Units;
using UnityEngine;

public class AllyView : UnitView
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ArrowView arrow))
        {
            arrow.ReturnToPool();
            ReceiveDamage(arrow.Damage);
        }
    }
}