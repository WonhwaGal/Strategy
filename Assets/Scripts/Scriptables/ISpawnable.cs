using UnityEngine;

public interface ISpawnable
{
    public PrefabType PrefabType { get; }
    public Transform Transform { get; }
}