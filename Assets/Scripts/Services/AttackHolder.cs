using UnityEngine;
using Code.Pools;

namespace Code.Combat
{
    public sealed class AttackHolder
    {
        private readonly ObjectMultiPool _weaponPool;
        private readonly Collider[] _colliders;

        public AttackHolder(WeaponList weaponList, Collider[] colliders)
        {
            _weaponPool = new ObjectMultiPool(weaponList);
            _colliders = colliders;
        }

        public void ShootArrow(int numberOfFounds, Vector3 origin)
        {
            var result = FindClosestOpponent(numberOfFounds, origin);
            ArrowView arrow = (ArrowView)_weaponPool.Spawn(PrefabType.Arrow);
            arrow.OnReachTarget += DespawnArrow;
            arrow.AssignTarget(origin, result);
        }

        public void SwordAreaAttack(int number, BasePresentorWaveCollection<IPresenter> participants)
        {
            for (int i = 0; i < number; i++)
                participants.ApplyDamage(_colliders[i].gameObject, _weaponPool.GetDamage(PrefabType.AreaSword));
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
            _weaponPool.Despawn(PrefabType.Arrow, arrow);
            arrow.OnReachTarget -= DespawnArrow;
        }
    }
}