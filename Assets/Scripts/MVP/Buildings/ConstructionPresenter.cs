using System;
using Code.Strategy;
using Code.Units;
using UnityEngine;

namespace Code.Construction
{
    public class ConstructionPresenter : IPresenter
    {
        private readonly ConstructionView _view;
        private readonly ConstructionModel _model;
        private readonly IConstructionStrategy _strategy;
        protected readonly HPBar _hpBar;

        public ConstructionPresenter(ConstructionView view, ConstructionModel model, IConstructionStrategy strategy)
        {
            _view = view;
            _model = model;
            _strategy = strategy;
            _hpBar = _view.HPBar;
            _hpBar.ChangeHPSlider(_model.Defense);
            _model.OnKilled += KillBuilding;
            _view.OnUpdate += Update;
            _view.OnModeChange += UpgradeStage;
            _view.OnTriggerAction += HandleTrigger;
            _view.OnViewDestroyed += Destroy;
            if (_model.AutoVisible)
                _view.ShowCurrentStage(1);
        }

        public event Action<IConstructionModel, BuildActionType> OnViewTriggered;
        public event Action<PrefabType, GameObject, IPresenter> OnReadyForCombat;
        public event Action<IPresenter, IUnitView> OnBeingKilled;
        public event Action<ConstructionPresenter> OnRequestDestroy;

        private void UpgradeStage(int currentStage)
        {
            _model.CurrentStage = currentStage;
            if (_model.PrefabType == PrefabType.Castle)
                ((CombatBuildingStrategy)_strategy).AssignCatle(_model.Transform.position);
        }

        private void HandleTrigger(BuildActionType action) => OnViewTriggered?.Invoke(_model, action);
        private void Update(float delta) => _strategy.Execute(_model, delta);

        public void ChangeStage(GameMode mode)
        {
            if (_model.CurrentStage <= 0)
                return;

            if (mode == GameMode.IsNight)
                OnReadyForCombat?.Invoke(_view.PrefabType, _view.gameObject, this);
            else if (mode == GameMode.IsDay)
                _view.ShowCurrentStage();
            _hpBar.gameObject.SetActive(mode == GameMode.IsNight);
            _strategy.IsNight = mode == GameMode.IsNight;
        }

        public void CheckOwnConnection(IConstructionModel model, BuildActionType action)
        {
            if (_strategy.IsNight || action == BuildActionType.Upgrade && 
                !model.AutoUpgrades[model.CurrentStage])
                return;

            if (_model.CurrentStage < 0 && model.ID == _model.ActivatedBy ||
                _model.CurrentStage >= 0 && model.ID == _model.ID)
                _view.React(action);
        }

        public void ReceiveDamage(int damage) => _hpBar.ChangeHPSlider(_model.Defense -= damage);

        public void KillBuilding()
        {
            _view.BrokenView.SetActive(true);
            _hpBar.gameObject.SetActive(false);
            //OnBeingKilled?.Invoke(this, _view);
        }

        public void Destroy() => OnRequestDestroy?.Invoke(this);
        public void Dispose()
        {
            _model.OnKilled -= KillBuilding;
            _view.OnUpdate -= Update;
            _view.OnModeChange -= UpgradeStage;
            _view.OnTriggerAction -= HandleTrigger;
            _model.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}