using Code.Combat;
using Code.Construction;
using Code.Units;

namespace Code.Strategy
{
    public sealed class CombatBuildingStrategy : IConstructionStrategy
    {
        private readonly CombatService _combatService;
        private float _currentInterval = 0;
        private bool _isCombatType;
        private bool _isDestroyed;

        public CombatBuildingStrategy(IConstructionPresenter presenter = null)
        {
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
            if (presenter != null)
                Init(presenter);
        }

        public void Init(IConstructionPresenter presenter)
        {
            var type = presenter.Model.PrefabType;
            _isCombatType = presenter.Model.IsForCombat;
            WaveLocator.ParticipateInCombat(type, presenter.View.gameObject, presenter);
        }

        public void Execute(IConstructionPresenter presenter, float delta)
        {
            if (_isDestroyed)
                return;

            if (_isCombatType && presenter.Model.CurrentStage > 0)
                CountDown(presenter.Model, delta);

            if (presenter.Model.IsDestroyed)
            {
                presenter.View.ShowDestroyedView();
                _isDestroyed = true;
            }
        }

        public void SwitchStrategy(IConstructionPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsDay)
                presenter.Strategy = new DayBuildingStrategy(presenter);
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