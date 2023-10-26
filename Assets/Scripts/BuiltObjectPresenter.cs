using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuiltStrategy
{
    void Execute(BuiltObjectModel model);
}

public class BuiltObjectPresenter : IDisposable
{
    private readonly BuiltObjectView View;
    private readonly BuiltObjectModel Model;
    private readonly IBuiltStrategy BuiltStrategy;

    public BuiltObjectPresenter(BuiltObjectView view, BuiltObjectModel model, IBuiltStrategy builtStrategy)
    {
        View = view;
        Model = model;
        BuiltStrategy = builtStrategy;
    }

    public void Dispose()
    {
        Model.Dispose();
    }
}

[RequireComponent(typeof(Rigidbody))]
public class BuiltObjectView : MonoBehaviour
{
    public event Action<bool> OnTriggerAction;
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerAction?.Invoke(true);
    }
    private void OnTriggerExit(Collider other)
    {
        OnTriggerAction?.Invoke(false);
    }
}

public class BuiltObjectModel : IDisposable
{
    public void Dispose()
    {

    }
}