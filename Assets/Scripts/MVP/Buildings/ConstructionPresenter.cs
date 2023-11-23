using System;
using Code.Combat;
using Code.Strategy;
using Code.UI;
using Code.Units;

namespace Code.Construction
{
    public class ConstructionPresenter : IConstructionPresenter
    {
        protected readonly ConstructionView _view;
        protected readonly ConstructionModel _model;
        private IConstructionStrategy _strategy;
        protected HPBar _hpBar;

        public ConstructionPresenter(ConstructionView view,
            ConstructionModel model, IConstructionStrategy strategy)
        {
            _view = view;
            _model = model;
            _strategy = strategy;

            _model.OnKilled += RuinBuilding;
            _view.OnUpdate += Update;
            _view.OnModeChange += UpgradeStage;
            _view.OnTriggerAction += HandleTrigger;
            _view.OnViewDestroyed += Destroy;
            ServiceLocator.Container.RequestFor<LevelService>()
                .OnChangingGameMode += OnGameModeChange;
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
        HPBar IConstructionPresenter.HPBar => _hpBar;

        public event Action<IConstructionModel, BuildActionType> OnViewTriggered;
        public event Action<IPresenter, IUnitView, bool> OnBeingKilled;
        public event Action<ConstructionPresenter> OnRequestDestroy;

        public void OnGameModeChange(GameMode mode) => _strategy.SwitchStrategy(this, mode);

        public void CheckOwnConnection(IConstructionModel model, BuildActionType action)
        {
            if (_strategy.GetType() != typeof(DayBuildingStrategy) || action == BuildActionType.Upgrade &&
                !model.AutoUpgrades[model.CurrentStage])
                return;

            if (_model.CurrentStage < 0 && model.ID == _model.ActivatedBy ||
                _model.CurrentStage >= 0 && model.ID == _model.ID)
            {
                _view.React(action);
                OnReactToUpgrade(action, model.ID == _model.ID);
            }
        }

        public void ReceiveDamage(int damage)
        {
            var newValue = _model.Defense -= damage;
            _hpBar.gameObject.SetActive(newValue > 0);
            _hpBar.SetHPValue(newValue);
        }

        public void SetUpHPBar(UIType uiType)
        {
            var hpPool = ServiceLocator.Container.RequestFor<HPBarPool>();
            _hpBar = hpPool.Spawn(uiType);
            hpPool.OnSpawned(_hpBar);
            _hpBar.SetUpSlider(_model.MaxHP, _model.Transform);
        }

        public void RuinBuilding(bool destroyView)
        {
            _model.IsDestroyed = true;
            OnBeingKilled?.Invoke(this, _view, destroyView);
        }

        private void Update(float delta) => _strategy.Execute(this, delta);
        private void UpgradeStage(int currentStage) => _model.CurrentStage = currentStage;
        private void HandleTrigger(BuildActionType action) => OnViewTriggered?.Invoke(_model, action);
        protected virtual void OnReactToUpgrade(BuildActionType action, bool selfBuild) { }

        public void Destroy(bool destroyView) => OnRequestDestroy?.Invoke(this);
        public void Dispose()
        {
            if (_hpBar != null)
                _hpBar.Despawn();
            _model.OnKilled -= RuinBuilding;
            _view.OnUpdate -= Update;
            _view.OnModeChange -= UpgradeStage;
            _view.OnTriggerAction -= HandleTrigger;
            _model.Dispose();
            ServiceLocator.Container.RequestFor<LevelService>()
                .OnChangingGameMode -= OnGameModeChange;
            GC.SuppressFinalize(this);
        }
    }
}