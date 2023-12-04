using Code.Construction;
using Code.Pools;
using UnityEngine;

namespace Code.Strategy
{
    public class BaseBuildingNightStrategy : IConstructionStrategy
    {
        private readonly RuinMultiPool _ruinsPool;
        protected RuinView _ruins;
        protected bool _isDestroyed;

        public BaseBuildingNightStrategy(IConstructionPresenter presenter)
        {
            _ruinsPool = ServiceLocator.Container.RequestFor<RuinMultiPool>();
            if (presenter != null)
                Init(presenter);
        }

        public virtual void Execute(IConstructionPresenter presenter, float delta) { }

        public void Init(IConstructionPresenter presenter)
        {
            var type = presenter.Model.PrefabType;
            if (presenter.Model.CurrentStage > 0)
                WaveLocator.ParticipateInCombat(type, presenter.View.gameObject, presenter);
            else
                presenter.View.gameObject.SetActive(false);
            presenter.Model.OnKilled += ConfirmDestruction;
            presenter.SetUpHPBar();
        }

        public void SwitchStrategy(IConstructionPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsDay)
            {
                if (_ruins != null)
                    _ruinsPool.Despawn(_ruins.PrefabType, _ruins);

                presenter.Strategy = new DayBuildingStrategy(presenter);
                presenter.HPBar.gameObject.SetActive(false);
            }
        }

        protected void SetUpRuins(IConstructionPresenter presenter)
        {
            _ruins = _ruinsPool.Spawn(presenter.Model.PrefabType);
            _ruinsPool.OnSpawned(_ruins, presenter.View);
            _ruins.SetStage(presenter.Model.CurrentStage);
            presenter.View.gameObject.SetActive(false);
            presenter.Model.IsDestroyed = true;
        }

        private void ConfirmDestruction() => _isDestroyed = true;
    }
}