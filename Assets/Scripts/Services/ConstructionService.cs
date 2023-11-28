using System;
using System.Collections.Generic;
using Code.Pools;
using Code.ScriptableObjects;
using UnityEngine;

namespace Code.Construction
{
    public sealed class ConstructionService : IService
    {
        private readonly ConstructionSO _constructionSO;
        private readonly ConstructionCreator _creator;
        private IConstructionModel _chosenModel;
        private bool _isNight;

        public ConstructionService(ConstructionSO constructionSO, ConstructionPrefabs prefabs)
        {
            _constructionSO = constructionSO;
            _creator = new ConstructionCreator(new ConstructionMultiPool(prefabs));
        }

        public event Action<IConstructionModel, BuildActionType> OnNotifyConnections;
        public event Action<Transform[], PrefabType> OnBuildingWithUnits;

        public void StartLevel(int lvlNumber)
            => CreatePresenters(_constructionSO.FindBuildingsOfLevel(lvlNumber));

        private void CreatePresenters(List<SingleBuildingData> buildings)
        {
            _isNight = false;
            if (buildings == null)
                return;

            for (int i = 0; i < buildings.Count; i++)
            {
                var presenter = _creator.CreatePresenter(buildings[i]);
                presenter.OnViewTriggered += SendTriggerNotification;
                presenter.OnRequestDestroy += DestroyPresenter;
                OnNotifyConnections += presenter.CheckOwnConnection;
                if (buildings[i].PrefabType == PrefabType.Barracks)
                    ((SpawnConstructionPresenter)presenter).OnBuildingWithUnits
                         += SendUnitSpawnRequest;
            }
        }

        public bool ReadyToBuild() // when Space key pressed
        {
            if (_chosenModel != null && _chosenModel.CurrentStage < _chosenModel.TotalStages)
            {
                NotifyOnTrigger();
                return true;
            }

            _isNight = true;
            return false;
        }

        public void Upgrade()
        {
            if (_chosenModel != null)
                SendTriggerNotification(_chosenModel, BuildActionType.Build);
        }

        private void NotifyOnTrigger()
        {
            if (_chosenModel.CurrentStage == 0)
                SendTriggerNotification(_chosenModel, BuildActionType.Build);
            else
                SendTriggerNotification(_chosenModel, BuildActionType.Upgrade);
        }

        private void SendTriggerNotification(IConstructionModel model, BuildActionType action)
        {
            if (_isNight || model.CurrentStage >= model.TotalStages - 1 && action != BuildActionType.Hide)
                return;

            OnNotifyConnections?.Invoke(model, action);
            _chosenModel = action == BuildActionType.Hide ? null : model;
        }

        private void SendUnitSpawnRequest(Transform[] spawnInfo) 
            => OnBuildingWithUnits?.Invoke(spawnInfo, PrefabType.Ally);

        private void DestroyPresenter(ConstructionPresenter presenter)
        {
            OnNotifyConnections -= presenter.CheckOwnConnection;
            presenter.OnRequestDestroy -= DestroyPresenter;
            presenter.OnViewTriggered -= SendTriggerNotification;
            presenter.Dispose();
        }
    }
}