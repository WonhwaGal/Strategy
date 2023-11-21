using System;
using System.Collections.Generic;
using Code.Pools;
using Code.ScriptableObjects;
using UnityEngine;

namespace Code.Construction
{
    public sealed class ConstructionService : IReactToDaytimeSwitch
    {
        private readonly ConstructionSO _constructionSO;
        private readonly ConstructionCreator _creator;
        private IConstructionModel _choosenModel;
        private bool _isNight;

        public ConstructionService(ConstructionSO constructionSO, ConstructionPrefabs prefabs)
        {
            _constructionSO = constructionSO;
            _creator = new ConstructionCreator(new ConstructionMultiPool(prefabs));
        }

        public event Action<IConstructionModel, BuildActionType> OnNotifyConnections;
        public event Action<GameMode> OnGameModeChange;

        public void StartLevel(int lvlNumber)
            => CreatePresenters(_constructionSO.FindBuildingsOfLevel(lvlNumber));

        private void CreatePresenters(List<SingleBuildingData> buildings)
        {
            if (buildings == null)
                return;

            for (int i = 0; i < buildings.Count; i++)
            {
                var presenter = _creator.CreatePresenter(buildings[i]);
                presenter.OnViewTriggered += SendTriggerNotification;
                presenter.OnRequestDestroy += DestroyPresenter;
                OnGameModeChange += presenter.OnGameModeChange;
                OnNotifyConnections += presenter.CheckOwnConnection;
            }
        }

        public void SwitchMode(GameMode mode)
        {
            _isNight = mode == GameMode.IsNight;
            OnGameModeChange?.Invoke(mode);
        }

        public bool ReadyToBuild() // when Space key pressed
        {
            if (_choosenModel != null && _choosenModel.CurrentStage < _choosenModel.TotalStages)
            {
                NotifyOnTrigger();
                return true;
            }
            return false;
        }

        public void Upgrade()
        {
            if (_choosenModel != null)
                SendTriggerNotification(_choosenModel, BuildActionType.Build);
        }

        private void NotifyOnTrigger()
        {
            if (_choosenModel.CurrentStage == 0)
                SendTriggerNotification(_choosenModel, BuildActionType.Build);
            else
                SendTriggerNotification(_choosenModel, BuildActionType.Upgrade);
        }

        private void SendTriggerNotification(IConstructionModel model, BuildActionType action)
        {
            if (_isNight)
                return;

            OnNotifyConnections?.Invoke(model, action);
            _choosenModel = action == BuildActionType.PutAway ? null : model;
        }

        private void DestroyPresenter(ConstructionPresenter presenter)
        {
            OnGameModeChange -= presenter.OnGameModeChange;
            OnNotifyConnections -= presenter.CheckOwnConnection;
            presenter.OnRequestDestroy -= DestroyPresenter;
            presenter.OnViewTriggered -= SendTriggerNotification;
            presenter.Dispose();
        }
    }
}