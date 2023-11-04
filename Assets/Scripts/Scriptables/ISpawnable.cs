using Code.ScriptableObjects;
using UnityEngine;

public interface ISpawnable
{
    public BuildingCommonData CommonInfo { get; }
    public Transform Transform { get; }
}