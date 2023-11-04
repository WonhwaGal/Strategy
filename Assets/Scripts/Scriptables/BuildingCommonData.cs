using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(BuildingCommonData), menuName = "Construction/BuildingCommonData")]
    public class BuildingCommonData : ScriptableObject
    {
        [field: SerializeField] public PrefabType PrefabType { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int TotalUpgradeStages { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
    }
}