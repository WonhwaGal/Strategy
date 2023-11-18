using System;
using System.Collections.Generic;
using UnityEngine;
using Code.Pools;
using Code.ScriptableObjects;

namespace Code.Construction
{
    public sealed class ConstructionService : ICombatService
    {
        private readonly ConstructionSO _constructionSO;
        private readonly ConstructionRegistry _registry;
        private IConstructionModel _choosenModel;
        private bool _isNight;

        public ConstructionService(ConstructionSO constructionSO, ConstructionPrefabs prefabs)
        {
            _constructionSO = constructionSO;
            _registry = new ConstructionRegistry(new ConstructionMultiPool(prefabs));
        }

        public event Action<IConstructionModel, BuildActionType> OnNotifyConnections;
        public event Action<PrefabType, GameObject, IPresenter> OnRegisterForCombat;
        public event Action<GameMode> OnGameModeChange;

        public void StartLevel(int lvlNumber)
            => CreatePresenters(_constructionSO.FindBuildingsOfLevel(lvlNumber));

        private void CreatePresenters(List<SingleBuildingData> buildings)
        {
            if (buildings == null)
                return;

            for (int i = 0; i < buildings.Count; i++)
            {
                var presenter = _registry.CreatePresenter(buildings[i]);
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

        public void RegisterForCombat(PrefabType type, GameObject go, IPresenter presenter)
            => OnRegisterForCombat?.Invoke(type, go, presenter);

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
            if (action == BuildActionType.PutAway)
                _choosenModel = null;
            else
                _choosenModel = model;
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