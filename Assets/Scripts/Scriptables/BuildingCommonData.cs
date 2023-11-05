using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(BuildingCommonData), menuName = "Construction/BuildingCommonData")]
    public class BuildingCommonData : ScriptableObject
    {
        public PrefabType PrefabType;
        public int Defense;
        public int TotalStages;
        [field: SerializeField] public bool[] AutoUpgrades { get; private set; }
        [field: SerializeField] public int[] PriceList { get; private set; }
    }
}