using UnityEngine;
using Code.Combat;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(WeaponList), menuName = "Weapons/WeaponList")]
    public sealed class WeaponList : PrefabSO<WeaponView>
    {
    }
}