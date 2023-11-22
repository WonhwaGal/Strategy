using Code.Units;

namespace Code.Strategy
{
    public class DayAllyStrategy : IUnitStrategy
    {
        public DayAllyStrategy(IUnitPresenter presenter = null)
        {
            if (presenter != null)
                Init(presenter);
        }

        public void Execute(IUnitPresenter presenter, float delta) { }

        public void Init(IUnitPresenter presenter)
            => presenter.View.NavAgent.speed = presenter.Model.Speed;

        public void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsNight)
                presenter.Strategy = new AllyInfantryStrategy(presenter);
            else if (mode == GameMode.IsUnitControl)
                presenter.Strategy = new UnitControlStrategy(presenter);
        }
    }

    public sealed class AllyInfantryStrategy : BaseInfantryStrategy
    {
        private float _currentInterval = 0;

        public AllyInfantryStrategy(IUnitPresenter presenter = null)
        {
            WaveLocator.ParticipateInCombat(presenter.Model.PrefabType, presenter.View.gameObject, presenter);
            if (presenter != null)
                Init(presenter);
        }

        public override void Execute(IUnitPresenter presenter, float delta)
            => Move(presenter.View, presenter.Model, delta);

        public override void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            base.SwitchStrategy(presenter, mode);
            if (mode == GameMode.IsDay)
                presenter.Strategy = new DayAllyStrategy(presenter);
        }

        protected override void Move(UnitView view, UnitModel model, float delta)
        {
            if (_target == null)
                SearchForNewOpponent(model);

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

    public sealed class UnitControlStrategy : IUnitStrategy
    {
        public UnitControlStrategy(IUnitPresenter presenter = null)
        {
            if (presenter != null)
                Init(presenter);
        }

        public void Execute(IUnitPresenter presenter, float delta)
        {

        }

        public void Init(IUnitPresenter presenter)
        {

        }

        public void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsNight)
                presenter.Strategy = new EnemyInfantryStrategy(presenter);
            else if (mode == GameMode.IsDay)
                presenter.Strategy = new DayAllyStrategy(presenter);
        }
    }
}