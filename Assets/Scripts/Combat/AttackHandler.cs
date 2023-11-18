using UnityEngine;
using Code.Units;
using Code.Tools;

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

        public static void SwordAreaAttack(IModel model, LayerMask mask, BaseWaveCollection<IPresenter> list)
        {
            if (list == null)
                return;

            var targetNumber = GetTargetsInRadius(model.Transform.position, model.LongRadius, mask);
            for (int i = 0; i < targetNumber; i++)
                list.ApplyDamage(_colliders[i].gameObject, ((UnitModel)model).Damage);
        }

        public static IPresenter FindClosestType(IModel model, LayerMask mask, BaseWaveCollection<IPresenter> list)
        {
            if (list == null)
                return null;

            var result = FindClosestOpponent(model, mask);

            if (result != null)
                return list.FindParticipant(result.gameObject);
            else if(mask.value == LayerMask.GetMask(Constants.Buildings))
                return ((BuildingWaveCollection)list).FindCastle();

            return null;
        }

        public static void HitTarget(IPresenter target, int damage) 
            => target.ReceiveDamage(damage);

        private static Transform FindClosestOpponent(IModel model, LayerMask mask)
        {
            var numberOfFounds = GetTargetsInRadius(model.Transform.position, model.LongRadius, mask);
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

        private static int GetTargetsInRadius(Vector3 origin, float radius, int mask)
            => Physics.OverlapSphereNonAlloc(origin, radius, _colliders, mask);
    }
}