using System;
using UnityEngine;

[Serializable]
public class SingleBuildingData : ISpawnable
{
    [SerializeField] private string Name;
    [field: SerializeField] public Transform Transform { get; private set; }
    [field: SerializeField] public PrefabType PrefabType { get; private set; }
    [field: SerializeField] public int Defense { get; private set; }
    [field: SerializeField] public bool AutoVisible { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public int ActivatedBy { get; private set; }
    [field: SerializeField] public int TotalUpgradeStages { get; private set; }
}