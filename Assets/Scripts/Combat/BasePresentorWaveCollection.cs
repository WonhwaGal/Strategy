using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePresentorWaveCollection<T> : IDisposable where T : IPresenter
{
    protected Dictionary<GameObject, T> _currentObjects = new();

    public void AddToCollection(GameObject go, T presenter)
    {

    }

    public void Dispose()
    {
        _currentObjects.Clear();
    }
}

public interface IPresenter
{
}