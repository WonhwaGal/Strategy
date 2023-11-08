using System;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [Serializable]
    public class SingleBuildingData : ISpawnBuilding
    {
        [SerializeField] private string Name;
        [field: SerializeField] public PrefabType PrefabType { get; private set; }
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public UniqueData UniqueInfo { get; private set; }
        [field: SerializeField] public BuildingCommonData CommonInfo { get; private set; }

        [Serializable]
        public sealed class UniqueData
        {
            [field: SerializeField] public bool AutoVisible { get; private set; }
            [field: SerializeField] public int ID { get; private set; }
            [field: SerializeField] public int ActivatedBy { get; private set; }
        }
    }
}