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
            HP = MaxHP = settings.HP;
            Speed = settings.Speed;
            DamageRadius = settings.DamageRadius;
            LongRadius = settings.LongRangeRadius;
            AttackInterval = settings.AttackInterval;
            Damage = settings.Damage;
        }

        public PrefabType PrefabType { get; private set; }
        public Transform Transform { get; private set; }
        public int MaxHP { get; private set; }
        public int HP
        {
            get => _hp;
            set
            {
                _hp = value;
                if (_hp <= 0)
                    OnKilled?.Invoke(false);
            }
        }
        public float Speed { get; set; }
        public float DamageRadius { get; private set; }
        public float LongRadius { get; private set; }
        public float AttackInterval { get; private set; }
        public int Damage { get; private set; }  // which DAMAGE is this?

        public event Action<bool> OnKilled;

        public void Dispose() { }
    }
}