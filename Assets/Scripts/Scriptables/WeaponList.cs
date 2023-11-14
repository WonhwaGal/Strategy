using System;
using System.Collections.Generic;
using UnityEngine;
using Code.Combat;

[CreateAssetMenu(fileName = nameof(WeaponList), menuName = "Weapons/WeaponList")]
public class WeaponList : ScriptableObject
{
    [SerializeField] private List<Weapon> _weapons;

    public WeaponView FindPrefab(PrefabType type)
    {
        var weapon = _weapons.Find(x => x.WeaponType == type);
        if (weapon != null)
            return weapon.WeaponPrefab;
        else
            Debug.LogError($"{name} : level #{weapon.WeaponType} is not found");

        return null;
    }

    [Serializable]
    public class Weapon
    {
        public string Name;
        public PrefabType WeaponType;
        public WeaponView WeaponPrefab;
    }
}