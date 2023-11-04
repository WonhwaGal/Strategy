using System;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [Serializable]
    public class SingleBuildingData : ISpawnable
    {
        [SerializeField] private string Name;
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public bool AutoVisible { get; private set; }
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public int ActivatedBy { get; private set; }
        [field: SerializeField] public BuildingCommonData CommonData { get; private set; }
    }
}