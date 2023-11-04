using System;
using System.Collections.Generic;
using Code.Factories;
using Code.Input;
using Code.ScriptableObjects;
using UnityEngine;

namespace Code.Construction
{
    public sealed class ConstructionService : IService
    {
        private readonly ConstructionSO _constructionSO;
        private readonly Pool<ConstructionView> _pool;
        private readonly PresenterRegistry _registry;
        private int _choosenConstructionID;

        public event Action<int, BuildActionType> OnNotifyConnections;
        public event Action<Vector3> OnBuildConstruction;

        public ConstructionService(ConstructionSO constructionSO, ConstructionPrefabs prefabs)
        {
            _constructionSO = constructionSO;
            _pool = new ConstructionPool<ConstructionView>(prefabs);
            _registry = new PresenterRegistry(_pool);
        }

        public void StartLevel(int lvlNumber) 
            => CreatePresenters(_constructionSO.FindBuildingsOfLevel(lvlNumber));

        private void CreatePresenters(List<SingleBuildingData> buildings)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                var presenter = _registry.CreatePresenter(buildings[i]);
                presenter.OnDestroyObj += DestroyBuilding;
                presenter.OnShowConnections += NotifyConnections;
                OnNotifyConnections += presenter.CheckConnection;
            }
        }

        private void NotifyConnections(int senderID, BuildActionType action)
        {
            _choosenConstructionID = action == BuildActionType.Show ? senderID : 0;
            OnNotifyConnections?.Invoke(senderID, action);
        }

        public void BuildConstruction()
        {
            if (_choosenConstructionID != 0)
                NotifyConnections(_choosenConstructionID, BuildActionType.Build);
        }

        private void DestroyBuilding(ConstructionPresenter presenter)
        {
            presenter.Dispose();
            presenter.OnDestroyObj -= DestroyBuilding;
            presenter.OnShowConnections -= NotifyConnections;
            OnNotifyConnections -= presenter.CheckConnection;
        }
    }
}