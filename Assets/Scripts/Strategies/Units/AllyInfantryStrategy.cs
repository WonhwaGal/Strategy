using Code.Units;

namespace Code.Strategy
{
    public sealed class AllyInfantryStrategy : BaseInfantryStrategy
    {
        public AllyInfantryStrategy(IUnitPresenter presenter = null) : base(presenter)
        {
        }

        public override void Execute(IUnitPresenter presenter, float delta)
            => Move(presenter.View, presenter.Model, delta);

        public override void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            base.SwitchStrategy(presenter, mode);
            if (mode == GameMode.IsDay)
            {
                presenter.HPBar.gameObject.SetActive(false);
                presenter.Strategy = new DayAllyStrategy(presenter);
            }
        }

        protected override void Move(UnitView view, UnitModel model, float delta)
        {
            if (_target != null)
                view.NavAgent.SetDestination(_target.position);

            Await(model, delta);
        }

        protected override void Await(UnitModel model, float delta)
        {
            _currentInterval += delta;
            if (_currentInterval >= model.AttackInterval)
            {
                SearchForNewOpponent(model);
                _currentInterval = 0;
            }
        }

        protected override void OnFindTarget(UnitModel model, IPresenter presenter, bool isUnit)
        {
            ReceiveTarget(((IUnitPresenter)presenter).Model);
            if (CheckAttackConditions(model.Transform.position))
                Attack(presenter, model.Damage);
        }
    }
}