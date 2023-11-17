using System;
using UnityEngine;
using Code.Units;

namespace Code.Combat
{
    public static class AttackHandler
    {
        private readonly static Collider[] _colliders = new Collider[25];

        public static void ShootArrow(IModel model, LayerMask mask, ArrowView arrow)
        {
            var result = FindClosestOpponent(model, mask);
            if (result != null)
                arrow.AssignTarget(model.Transform.position, result);
            else
                arrow.ReturnToPool();
        }

        public static void SwordAreaAttack(IModel model, LayerMask mask, BaseWaveCollection<IPresenter> participants)
        {
            var targetNumber = GetTargetsInRadius(model, mask);
            for (int i = 0; i < targetNumber; i++)
                participants.ApplyDamage(_colliders[i].gameObject, ((UnitModel)model).Damage);
        }

        private static Transform FindClosestOpponent(IModel model, LayerMask mask)
        {
            var numberOfFounds = GetTargetsInRadius(model, mask);
            if (numberOfFounds == 0)
                return null;

            Collider closest = _colliders[0];
            if (numberOfFounds == 1)
                return closest.transform;

            var origin = model.Transform.position;
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

        private static int GetTargetsInRadius(IModel model, int mask)
            => Physics.OverlapSphereNonAlloc(model.Transform.position, model.Radius, _colliders, mask);
    }
}