using System;
using UnityEngine;
using Code.Pools;

namespace Code.Units
{
    public interface IUnitView : ISpawnableType
    {
        event Action<float> OnUpdate;
        event Action<bool> OnViewDestroyed;

        GameObject GameObject { get; }
    }
}