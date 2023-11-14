using System;
using UnityEngine;

namespace Code.Units
{
    public interface IModel : IDisposable
    {
        Transform Transform { get; }
        PrefabType PrefabType { get; }
        float Radius { get; }

        event Action OnKilled;
    }
}