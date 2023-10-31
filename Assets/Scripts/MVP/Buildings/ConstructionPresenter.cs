using System;
using UnityEngine;

namespace Code.Construction
{
    public class ConstructionPresenter : IDisposable
    {
        private readonly ConstructionView View;
        private readonly ConstructionModel Model;
        private readonly IConstructionStrategy BuiltStrategy;

        public event Action<ConstructionPresenter> OnDestroyObj;

        public ConstructionPresenter(ConstructionView view, ConstructionModel model, IConstructionStrategy builtStrategy)
        {
            View = view;
            Model = model;
            BuiltStrategy = builtStrategy;
            Model.OnDestroyed += DestroyObject;
            Debug.Log("build presenter with " + view.name);
        }

        public void DestroyObject()
        {
            Model.OnDestroyed -= DestroyObject;
            OnDestroyObj?.Invoke(this);
        }

        public void Dispose()
        {
            Model.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}