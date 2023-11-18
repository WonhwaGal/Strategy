﻿using System;
using UnityEngine;

namespace Code.Units
{
    public interface IModel : IDisposable
    {
        Transform Transform { get; }
        PrefabType PrefabType { get; }
        float CloseRadius { get; }
        float LongRadius { get; }

        event Action OnKilled;
    }
}