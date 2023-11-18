using System;
using UnityEngine;
using static UnitSettingList;

namespace Code.Units
{
    public class UnitModel : IModel
    {
        private int _hp;

        public UnitModel(UnitSettings settings, Transform transform)
        {
            PrefabType = settings.PrefabType;
            Transform = transform;
            HP = settings.HP;
            Speed = settings.Speed;
            CloseRadius = settings.CloseRangeRadius;
            LongRadius = settings.LongRangeRadius;
            AttackInterval = settings.AttackInterval;
            Damage = settings.Damage;
        }

        public PrefabType PrefabType { get; private set; }
        public Transform Transform { get; private set; }
        public int HP
        {
            get => _hp;
            set
            {
                _hp = value;
                if(_hp <= 0)
                {
                    OnKilled?.Invoke();
                }
            }
        }
        public float Speed { get; set; }
        public float CloseRadius { get; private set; }
        public float LongRadius { get; private set; }
        public float AttackInterval { get; private set; }
        public int Damage { get; private set; }  // which DAMAGE is this?

        public event Action OnKilled;

        public void Dispose() { }
    }
}