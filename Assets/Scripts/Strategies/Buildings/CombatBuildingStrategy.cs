using Code.Combat;
using Code.Construction;
using Code.Units;

namespace Code.Strategy
{
    public sealed class CombatBuildingStrategy : BaseBuildingNightStrategy
    {
        private readonly CombatService _combatService;
        private float _currentInterval = 0;

        public CombatBuildingStrategy(IConstructionPresenter presenter = null) : base(presenter)
        {
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
        }

        public override void Execute(IConstructionPresenter presenter, float delta)
        {
            if (_isDestroyed && _ruins == null)
                SetUpRuins(presenter);

            if (presenter.Model.CurrentStage > 0)
                CountDown(presenter.Model, delta);
        }

        private void CountDown(ConstructionModel model, float delta)
        {
            _currentInterval += delta;
            if (_currentInterval >= model.AttackInterval)
            {
                Attack(model, AttackType.Arrow);
                _currentInterval = 0;
            }
        }

        private void Attack(IModel model, AttackType attack)
            => _combatService.CheckForTargets(model, attack);
    }
}