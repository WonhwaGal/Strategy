using Code.Combat;
using Code.Construction;
using UnityEngine;

namespace Code.Strategy
{
    public sealed class DayBuildingStrategy : IConstructionStrategy
    {
        public DayBuildingStrategy(IConstructionPresenter presenter = null)
        {
            //get coin pool
            if (presenter != null)
                Init(presenter);
        }

        public void Init(IConstructionPresenter presenter)
        {
            // if building is destroyed - Does it give a coin?
            Recover(presenter.Model, presenter.View);
            GrantCoin(presenter.Model);
            if (presenter.Model.PrefabType == PrefabType.Castle)
                ServiceLocator.Container.RequestFor<CombatService>().Castle = presenter.Model;
        }

        public void Execute(IConstructionPresenter presenter, float delta) { }

        public void SwitchStrategy(IConstructionPresenter presenter, GameMode mode)
        {
            var stage = presenter.Model.CurrentStage;
            if (mode == GameMode.IsNight && stage > 0)
                presenter.Strategy = new CombatBuildingStrategy(presenter);
        }

        private void GrantCoin(ConstructionModel model)
        {
            if (model.PrefabType == PrefabType.House || model.PrefabType == PrefabType.Mill)
                Debug.Log(model.CurrentStage + " coins granted");
        }

        private void Recover(ConstructionModel model, ConstructionView view)
        {
            if (model.IsDestroyed)
            {
                view.ShowCurrentStage();
                model.IsDestroyed = false;
            }
        }
    }
}