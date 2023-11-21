using Code.Construction;
using Code.Pools;

namespace Code.Strategy
{
    public class BaseBuildingNightStrategy : IConstructionStrategy
    {
        private readonly RuinMultiPool _ruinsPool;
        private RuinView _ruins;
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
        }

        public void SwitchStrategy(IConstructionPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsDay)
            {
                if (_ruins != null)
                    _ruinsPool.Despawn(PrefabType.CommonRuin, _ruins);
                presenter.Strategy = new DayBuildingStrategy(presenter);
            }
        }

        protected void SetUpRuins(IConstructionPresenter presenter)
        {
            _isDestroyed = true;
            _ruins = _ruinsPool.Spawn(PrefabType.CommonRuin);
            _ruinsPool.OnSpawned(_ruins, presenter.View);
            presenter.View.gameObject.SetActive(false);
        }
    }
}