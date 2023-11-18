using System;
using Code.Strategy;
using Code.Units;

namespace Code.Construction
{
    public class ConstructionPresenter : IConstructionPresenter
{
        private readonly ConstructionView _view;
        private readonly ConstructionModel _model;
        private IConstructionStrategy _strategy;
        protected readonly HPBar _hpBar;

        public ConstructionPresenter(ConstructionView view, ConstructionModel model, IConstructionStrategy strategy)
        {
            _view = view;
            _model = model;
            _strategy = strategy;
            _hpBar = _view.HPBar;
            _hpBar.ChangeHPSlider(_model.Defense);
            _model.OnKilled += RuinBuilding;
            _view.OnUpdate += Update;
            _view.OnModeChange += UpgradeStage;
            _view.OnTriggerAction += HandleTrigger;
            _view.OnViewDestroyed += Destroy;
            if (_model.AutoVisible)
                _view.ShowCurrentStage(1);
        }

        ConstructionView IConstructionPresenter.View => _view;
        ConstructionModel IConstructionPresenter.Model => _model;
        IStrategy IConstructionPresenter.Strategy 
        { 
            get => _strategy; 
            set 
            { 
                _strategy = (IConstructionStrategy)value; 
            } 
        }

        public event Action<IConstructionModel, BuildActionType> OnViewTriggered;
        public event Action<IPresenter, IUnitView> OnBeingKilled;
        public event Action<ConstructionPresenter> OnRequestDestroy;

        public void OnGameModeChange(GameMode mode) => _strategy.SwitchStrategy(this, mode);

        public void CheckOwnConnection(IConstructionModel model, BuildActionType action)
        {
            if (_strategy.GetType() != typeof(DayBuildingStrategy) || action == BuildActionType.Upgrade &&
                !model.AutoUpgrades[model.CurrentStage])
                return;

            if (_model.CurrentStage < 0 && model.ID == _model.ActivatedBy ||
                _model.CurrentStage >= 0 && model.ID == _model.ID)
                _view.React(action);
        }

        public void ReceiveDamage(int damage) => _hpBar.ChangeHPSlider(_model.Defense -= damage);

        public void RuinBuilding()
        {
            _model.IsDestroyed = true;
            _hpBar.gameObject.SetActive(false);
            OnBeingKilled?.Invoke(this, _view);
        }

        private void Update(float delta) => _strategy.Execute(this, delta);
        private void UpgradeStage(int currentStage) => _model.CurrentStage = currentStage;
        private void HandleTrigger(BuildActionType action) => OnViewTriggered?.Invoke(_model, action);

        public void Destroy() => OnRequestDestroy?.Invoke(this);
        public void Dispose()
        {
            _model.OnKilled -= RuinBuilding;
            _view.OnUpdate -= Update;
            _view.OnModeChange -= UpgradeStage;
            _view.OnTriggerAction -= HandleTrigger;
            _model.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}