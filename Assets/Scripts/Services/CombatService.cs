using UnityEngine;
using Code.Units;
using Code.Pools;
using Code.Tools;
using Code.ScriptableObjects;

namespace Code.Combat
{
    public sealed class CombatService : IService
    {
        private readonly ObjectMultiPool _weaponPool;
        private LayerMask _enemyMask;
        private LayerMask _playerMask;
        private LayerMask _buildingMask;
        private LayerMask _castleMask;
        private LayerMask _allyMask;

        public CombatService(WeaponList weaponList)
        {
            _weaponPool = new ObjectMultiPool(weaponList);
            _enemyMask = LayerMask.GetMask(Constants.Enemies);
            _playerMask = LayerMask.GetMask(Constants.Player);
            _buildingMask = LayerMask.GetMask(Constants.Buildings);
            _castleMask = LayerMask.GetMask(Constants.Buildings);
            _allyMask = LayerMask.GetMask(Constants.Allies);
        }

        public IModel Castle { get; set; }

        public (IPresenter, bool) ReceiveClosestTarget(IModel model, bool onlyCastle = false)
        {
            bool targetIsUnit = false;
            if (!onlyCastle)
            {
                var mask = GetMask(model.PrefabType, allMasks: false, amongUnits: true);
                var result = AttackHandler
                    .FindClosestType(model, mask, WaveLocator.GetCollection(Constants.Allies));

                if (result != null)
                    return (result, true);
            }

            var buildingMask = onlyCastle ? _castleMask : GetMask(model.PrefabType, allMasks: false);
            var finalResult = AttackHandler
                .FindClosestType(model, buildingMask, WaveLocator.GetCollection(Constants.Buildings));
            return (finalResult, targetIsUnit);
        }

        public void CheckForTargets(IModel model, AttackType attack)
        {
            var mask = GetMask(model.PrefabType, true);

            switch (attack)
            {
                case AttackType.Arrow:
                    AttackHandler.ShootArrow(model, mask, GetArrow());
                    break;
                case AttackType.AreaSword:
                    AttackHandler.SwordAreaAttack(model, mask, WaveLocator.GetCollection(Constants.Enemies));
                    break;
            }
        }

        public void HitTarget(IPresenter target, int damage)
            => target.ReceiveDamage(damage);

        private LayerMask GetMask(PrefabType type, bool allMasks, bool amongUnits = false)
        {
            if ((int)type <= (int)PrefabType.Player)
                return _enemyMask;

            if (allMasks)
                return _playerMask | _allyMask | _buildingMask | _castleMask;
            else if (amongUnits)
                return _playerMask | _allyMask;
            else
                return _buildingMask | _castleMask;
        }

        private ArrowView GetArrow()
        {
            var arrow = (ArrowView)_weaponPool.Spawn(PrefabType.Arrow);
            arrow.OnReachTarget += DespawnArrow;
            return arrow;
        }

        private void DespawnArrow(ArrowView arrow)
        {
            _weaponPool.Despawn(PrefabType.Arrow, arrow);
            arrow.OnReachTarget -= DespawnArrow;
        }
    }
}