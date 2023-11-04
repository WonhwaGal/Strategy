using Code.ScriptableObjects;
using UnityEngine;

public interface ISpawnable
{
    public BuildingCommonData CommonData { get; }
    public Transform Transform { get; }
}