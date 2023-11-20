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
        private LayerMask _allyMask;

        public CombatService(WeaponList weaponList)
        {
            _weaponPool = new ObjectMultiPool(weaponList);
            _enemyMask = LayerMask.GetMask(Constants.Enemies);
            _playerMask = LayerMask.GetMask(Constants.Player);
            _buildingMask = LayerMask.GetMask(Constants.Buildings);
            _allyMask = LayerMask.GetMask(Constants.Allies);
        }

        public IModel Castle { get; set; }

        public (IPresenter, bool) ReceiveClosestTarget(IModel model)
        {
            bool isUnit = false;
            var result = AttackHandler
                .FindClosestType(model, _playerMask | _allyMask, WaveLocator.GetCollection(Constants.Allies));
            if(result == null)
            {
                result = AttackHandler
                    .FindClosestType(model, _buildingMask, WaveLocator.GetCollection(Constants.Buildings));
                return (result, isUnit);
            }
            else
            {
                return (result, true);
            }
        }

        public void CheckForTargets(IModel model, AttackType attack)
        {
            var mask = GetMask(model.PrefabType);

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
            => AttackHandler.HitTarget(target, damage);

        private LayerMask GetMask(PrefabType type)
        {
            if ((int)type <= (int)PrefabType.Player)
                return _enemyMask;
            else
                return _playerMask | _buildingMask | _allyMask;
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