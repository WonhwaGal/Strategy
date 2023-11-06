using System;
using UnityEngine;
using static UnitSettingList;

namespace Code.Units
{
    public class UnitModel : IModel
    {
        public UnitModel(UnitSettings settings, Transform transform)
        {
            PrefabType = settings.PrefabType;
            Transform = transform;
            HP = settings.HP;
            Speed = settings.Speed;
            Radius = settings.LongRangeRadius;
            AttackInterval = settings.AttackInterval;
        }

        public PrefabType PrefabType { get; private set; }
        public Transform Transform { get; private set; }
        public int HP { get; set; }
        public float Speed { get; set; }
        public float Radius { get; private set; }
        public float AttackInterval { get; private set; }

        public void Dispose() { }
    }
}