using System;
using Code.Strategy;
using static Code.ScriptableObjects.SingleBuildingData;

namespace Code.Construction
{
    public class ConstructionPresenter : IDisposable
    {
        private readonly ConstructionView _view;
        private readonly ConstructionModel _model;
        private readonly IConstructionStrategy _strategy;

        public event Action<ConstructionPresenter> OnDestroyObj;
        public event Action<IConstructionModel, BuildActionType> OnViewTriggered;

        public ConstructionPresenter(ConstructionView view, ConstructionModel model, IConstructionStrategy strategy)
        {
            _view = view;
            _model = model;
            _strategy = strategy;
            _model.OnDestroyed += DestroyConstruction;
            _view.OnStageChange += UpgradeStage;
            _view.OnTriggerAction += HandleTrigger;
            if (_model.AutoVisible)
                _view.ShowCurrentStage(1);
        }

        private void UpgradeStage(int currentStage) => _model.CurrentStage = currentStage;
        private void HandleTrigger(BuildActionType action) => OnViewTriggered?.Invoke(_model, action);

        public void CheckOwnConnection(IConstructionModel model, BuildActionType action)
        {
            if (action == BuildActionType.Upgrade && !model.AutoUpgrades[model.CurrentStage]) 
                    return;
           
            if(_model.CurrentStage < 0 && model.ID == _model.ActivatedBy ||
                _model.CurrentStage >= 0 && model.ID == _model.ID)
                    _view.React(action);
        }

        public void DestroyConstruction()
        {
            _model.OnDestroyed -= DestroyConstruction;
            _view.OnStageChange -= UpgradeStage;
            _view.OnTriggerAction -= HandleTrigger;
            OnDestroyObj?.Invoke(this);
        }

        public ConstructionPresenter Clone(ConstructionView view, UniqueData data)
        {
            var model = _model.Clone() as ConstructionModel;
            model.ID = data.ID;
            model.ActivatedBy = data.ActivatedBy;
            model.AutoVisible = data.AutoVisible;
            var strategy = _strategy.Clone() as IConstructionStrategy;
            return new ConstructionPresenter(view, model, strategy);
        }

        public void Dispose()
        {
            _model.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}