using System;
using UnityEngine;

[Serializable]
public class SingleBuildingData : ISpawnable
{
    public string Name;
    [field: SerializeField] public Transform Transform { get; set; }
    [field: SerializeField] public PrefabType PrefabType { get; set; }

    public int QueueOrder;
    public int Defense;
    public int TotalUpgradeStages;
}
