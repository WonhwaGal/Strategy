using System;
using System.Collections.Generic;
using Code.Pools;
using Code.ScriptableObjects;

namespace Code.Construction
{
    public sealed class ConstructionService : IService
    {
        private readonly ConstructionSO _constructionSO;
        private readonly PresenterRegistry _registry;
        private IConstructionModel _choosenModel;

        public event Action<IConstructionModel, BuildActionType> OnNotifyConnections;

        public ConstructionService(ConstructionSO constructionSO, ConstructionPrefabs prefabs)
        {
            _constructionSO = constructionSO;
            _registry = new PresenterRegistry(new ConstructionMultiPool(prefabs));
        }

        public void StartLevel(int lvlNumber)
            => CreatePresenters(_constructionSO.FindBuildingsOfLevel(lvlNumber));

        private void CreatePresenters(List<SingleBuildingData> buildings)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                var presenter = _registry.CreatePresenter(buildings[i]);
                presenter.OnDestroyObj += DestroyBuilding;
                presenter.OnViewTriggered += SendTriggerNotification;
                OnNotifyConnections += presenter.CheckOwnConnection;
            }
        }

        public void TryToBuild() // when Space key pressed
        {
            if (_choosenModel == null || _choosenModel.CurrentStage >= _choosenModel.TotalStages)
                return;

            if(_choosenModel.CurrentStage == 0)
                SendTriggerNotification(_choosenModel, BuildActionType.Build);
            else
                SendTriggerNotification(_choosenModel, BuildActionType.Upgrade);
        }

        public void Upgrade()
        {
            if (_choosenModel != null)
                SendTriggerNotification(_choosenModel, BuildActionType.Build);
        }

        private void SendTriggerNotification(IConstructionModel model, BuildActionType action)
        {
            OnNotifyConnections?.Invoke(model, action);
            if (action == BuildActionType.PutAway)
                _choosenModel = null;
            else
                _choosenModel = model;
        }

        private void DestroyBuilding(ConstructionPresenter presenter)
        {
            presenter.Dispose();
            presenter.OnDestroyObj -= DestroyBuilding;
            presenter.OnViewTriggered -= SendTriggerNotification;
            OnNotifyConnections -= presenter.CheckOwnConnection;
        }
    }
}