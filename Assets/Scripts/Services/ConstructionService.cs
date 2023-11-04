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
        private int _choosenConstructionID;

        public event Action<int, BuildActionType> OnNotifyConnections;
        public event Action<Vector3> OnBuildConstruction;

        public ConstructionService(ConstructionSO constructionSO, ConstructionPrefabs prefabs)
        {
            _constructionSO = constructionSO;
            _pool = new ConstructionPool<ConstructionView>(prefabs);
        }

        public void StartLevel(int lvlNumber)
        {
            List<SingleBuildingData> buildings = _constructionSO.FindBuildingsOfLevel(lvlNumber);
            CreatePresenters(buildings);
        }

        private void CreatePresenters(List<SingleBuildingData> buildings)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                ConstructionView buildingView = _pool.Spawn(buildings[i]);
                var model = new ConstructionModel(buildings[i]);
                var strategy = AssignStrategy(buildings[i].CommonData.PrefabType);

                var presenter = new ConstructionPresenter(buildingView, model, strategy);
                presenter.OnDestroyObj += DestroyBuilding;
                presenter.OnShowConnections += NotifyConnections;
                OnNotifyConnections += presenter.CheckConnection;
            }
        }
        private IConstructionStrategy AssignStrategy(PrefabType type) => new TestStrategy();

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