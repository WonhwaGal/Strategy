using System;
using UnityEngine;
using Code.Units;

namespace Code.Combat
{
    public sealed class CombatService : IService
    {
        private readonly AttackHolder _weaponHolder;
        private readonly Collider[] _colliders;
        private LayerMask _enemyMask;
        private LayerMask _playerMask;
        private LayerMask _buildingMask;
        private LayerMask _allyMask;
        private const int EnemyStartIndex = 30;

        private BasePresentorWaveCollection<IPresenter> _enemies;
        private BasePresentorWaveCollection<IPresenter> _allies;

        public CombatService(WeaponList weaponList)
        {
            _colliders = new Collider[25];
            _weaponHolder = new (weaponList, _colliders);
            _enemyMask = LayerMask.GetMask("Enemies");
            _playerMask = LayerMask.GetMask("Player");
            _buildingMask = LayerMask.GetMask("Building");
            _allyMask = LayerMask.GetMask("Allies");
        }

        public Vector3 Castle { get; set; }

        public event Action<bool> OnCombatEnded;

        public void RegisterParticipants(PrefabType type, GameObject go, IPresenter presenter)
        {
            if ((int)type >= EnemyStartIndex)
                AddEnemy(go, presenter);
            else
                AddAlly(type, go, presenter);
        }

        private void AddEnemy(GameObject go, IPresenter presenter)
        {
            if (_enemies == null)
            {
                _enemies = new EnemyWaveCollection();
                _enemies.OnWaveOver += OnNightOver;
            }
            _enemies.AddToCollection(go, presenter);
        }

        private void AddAlly(PrefabType type, GameObject go, IPresenter presenter)
        {
            if (_allies == null)
            {
                _allies = new AlliesWaveCollection();
                _allies.OnWaveOver += OnNightOver;
            }

            if (type == PrefabType.Castle)
                ((AlliesWaveCollection)_allies).AddCastle(presenter);
            else
                _allies.AddToCollection(go, presenter);
        }

        public void CheckForTargets(IModel model, AttackType attack)
        {
            if ((int)model.PrefabType <= (int)PrefabType.Player)
                ScanArea(model.Transform.position, model.Radius, _enemyMask, attack);
            else
                ScanArea(model.Transform.position, model.Radius, _playerMask | _buildingMask | _allyMask, attack);
        }

        private void ScanArea(Vector3 origin, float radius, int mask, AttackType attack)
        {
            int number = Physics.OverlapSphereNonAlloc(origin, radius, _colliders, mask);
            if (number == 0)
                return;

            switch (attack)
            {
                case AttackType.Arrow:
                    _weaponHolder.ShootArrow(number, origin);
                    break;
                case AttackType.AreaSword:
                    _weaponHolder.SwordAreaAttack(number, _enemies);
                    break;
            }
        }

        private void OnNightOver(bool isVictory)
        {
            if (isVictory)
            {
                Debug.Log("Wave is defeated");
                _enemies.Dispose();
                _allies.Dispose();
                _enemies = null;
                _allies = null;
            }
            else
            {
                Debug.Log("Wave has destroyed the castle");
            }
            OnCombatEnded?.Invoke(isVictory);
        }
    }
}