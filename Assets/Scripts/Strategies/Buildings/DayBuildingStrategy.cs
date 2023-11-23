using Code.Combat;
using Code.Construction;
using UnityEngine;

namespace Code.Strategy
{
    public sealed class DayBuildingStrategy : IConstructionStrategy
    {
        private bool _isCombatType;

        public DayBuildingStrategy(IConstructionPresenter presenter = null)
        {
            //get coin pool
            if (presenter != null)
                Init(presenter);
        }

        public void Init(IConstructionPresenter presenter)
        {
            presenter.View.gameObject.SetActive(true);
            _isCombatType = presenter.Model.IsForCombat;

            if (presenter.Model.CurrentStage > 0)
                CheckForRecover(presenter.Model, presenter.View);

            if (presenter.Model.PrefabType == PrefabType.Castle)
                ServiceLocator.Container.RequestFor<CombatService>().Castle = presenter.Model;
        }

        public void Execute(IConstructionPresenter presenter, float delta) { }

        public void SwitchStrategy(IConstructionPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsNight)
            {
                presenter.Strategy = _isCombatType ?
                    new CombatBuildingStrategy(presenter) : new PassiveNightStrategy(presenter);
                var isCastle = presenter.Model.PrefabType == PrefabType.Castle;
                presenter.SetUpHPBar(isCastle ? UIType.CastleHP : UIType.BuildingHP);
            }
        }

        private void GrantCoin(ConstructionModel model)
        {
            if (model.PrefabType == PrefabType.House || model.PrefabType == PrefabType.Mill)
                Debug.Log(model.CurrentStage + " coins granted");
        }

        private void CheckForRecover(ConstructionModel model, ConstructionView view)
        {
            if (model.IsDestroyed)
            {
                view.ShowCurrentStage();
                model.IsDestroyed = false;
            }
            else
            {
                GrantCoin(model);
            }
        }
    }
}