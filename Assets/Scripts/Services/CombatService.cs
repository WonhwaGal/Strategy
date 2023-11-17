using UnityEngine;
using Code.Units;
using Code.Pools;

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
            _enemyMask = LayerMask.GetMask("Enemies");
            _playerMask = LayerMask.GetMask("Player");
            _buildingMask = LayerMask.GetMask("Building");
            _allyMask = LayerMask.GetMask("Allies");
        }

        public Vector3 Castle { get; set; }

        public void CheckForTargets(IModel model, AttackType attack)
        {
            var mask = GetMask(model.PrefabType);

            switch (attack)
            {
                case AttackType.Arrow:
                    AttackHandler.ShootArrow(model, mask, GetArrow());
                    break;
                case AttackType.AreaSword:
                    AttackHandler.SwordAreaAttack(model, mask, WaveLocator.GetCollection("Enemy"));
                    break;
            }
        }

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