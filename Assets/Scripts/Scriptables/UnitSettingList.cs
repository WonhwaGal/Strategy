using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(UnitSettingList), menuName = "Units/UnitSettingSO")]
public class UnitSettingList : ScriptableObject
{
    [SerializeField] private List<UnitSettings> _units;

    public UnitSettings FindUnit(PrefabType type)
    {
        var unit = _units.Find(x => x.PrefabType == type);
        if (unit != null)
            return unit;
        else
            Debug.LogError($"{name} : level #{unit.Name} is not found");

        return null;
    }

    [Serializable]
    public class UnitSettings
    {
        public string Name;
        public PrefabType PrefabType;
        public int HP;
        public float Speed;
        public float CloseRangeRadius;
        public float LongRangeRadius;
        public float AttackInterval;
        public int Damage;
    }
}