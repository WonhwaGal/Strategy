using Code.Strategy;
using System;
using UnityEngine;
using static Code.ScriptableObjects.SingleBuildingData;

namespace Code.Construction
{
    public class ConstructionPresenter : IDisposable
    {
        private readonly ConstructionView View;
        private readonly ConstructionModel Model;
        private readonly IConstructionStrategy BuiltStrategy;

        public event Action<ConstructionPresenter> OnDestroyObj;
        public event Action<int, BuildActionType> OnShowConnections;

        public ConstructionPresenter(ConstructionView view, ConstructionModel model, IConstructionStrategy builtStrategy)
        {
            View = view;
            Model = model;
            BuiltStrategy = builtStrategy;
            Model.OnDestroyed += DestroyObject;
            View.OnTriggerAction += ShowConnectedObjects;
            if (Model.AutoVisible)
                View.ShowCurrentStage(1);
            Debug.Log($"created presenter for id {Model.ID}, activated by {Model.ActivatedBy} and auto {Model.AutoVisible}");
        }

        private void ShowConnectedObjects(BuildActionType action) => OnShowConnections?.Invoke(Model.ID, action);

        public void CheckConnection(int senderID, BuildActionType action)
        {
            if (senderID == Model.ActivatedBy || senderID == Model.ID)
            {
                View.gameObject.SetActive(true);
                View.Show(action);
            }
        }

        public void DestroyObject()
        {
            Model.OnDestroyed -= DestroyObject;
            View.OnTriggerAction -= ShowConnectedObjects;
            OnDestroyObj?.Invoke(this);
        }

        public ConstructionPresenter Clone(ConstructionView view, UniqueData data)
        {
            var model = Model.Clone(data.ID, data.ActivatedBy, data.AutoVisible);
            var strategy = BuiltStrategy.Clone() as IConstructionStrategy;
            return new ConstructionPresenter(view, model, strategy);
        }

        public void Dispose()
        {
            Model.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}