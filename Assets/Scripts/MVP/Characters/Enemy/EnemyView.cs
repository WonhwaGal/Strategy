using UnityEngine;

namespace Code.Units
{
    public class EnemyView : UnitView
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArrowView arrow))
            {
                arrow.ReturnToPool();
                ReceiveDamage();
            }
        }
    }
}