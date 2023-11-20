using Code.Construction;

namespace Code.Strategy
{
    public sealed class PassiveNightStrategy : BaseBuildingNightStrategy
    {
        public PassiveNightStrategy(IConstructionPresenter presenter) : base(presenter) { }

        public override void Execute(IConstructionPresenter presenter, float delta)
        {
            if (_isDestroyed)
                return;

            if (presenter.Model.IsDestroyed)
                SetUpRuins(presenter);
        }
    }
}