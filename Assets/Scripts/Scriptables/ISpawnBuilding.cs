﻿using Code.Pools;
using Code.ScriptableObjects;
using UnityEngine;

public interface ISpawnBuilding : ISpawnableType
{
    public BuildingCommonData CommonInfo { get; }
    public Transform Transform { get; }
}