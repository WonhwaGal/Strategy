using Code.Units;

namespace Code.Strategy
{
    public sealed class AllyInfantryStrategy : BaseInfantryStrategy
    {
        private readonly bool _isStatic;

        public AllyInfantryStrategy(IUnitPresenter presenter = null) : base(presenter)
        {
            if (presenter != null)
                _isStatic = ((AllyUnit)presenter.View).IsStaticInCombat;
        }

        public override void Execute(IUnitPresenter presenter, float delta)
        {
            if (_stopActions)
            {
                presenter.View.NavAgent.isStopped = true;
                return;
            }

            if (_isStatic)
                presenter.View.transform.LookAt(_target);
            else
                Move(presenter.View, presenter.Model, delta);

            Await(presenter.Model, delta);
        }

        public override void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsNight)
                WaveLocator.ParticipateInCombat(
                    presenter.Model.PrefabType, presenter.View.GameObject, presenter);
            else if(presenter.HPBar != null)
                presenter.HPBar.gameObject.SetActive(false);

            if (mode == GameMode.IsDay)
                presenter.Strategy = new DayAllyStrategy(presenter);
        }

        protected override void Move(UnitView view, UnitModel model, float delta)
        {
            if (_target != null)
                view.NavAgent.SetDestination(_target.position);
            _animator.AnimateMovement(view.NavAgent.hasPath);
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
            if (IsTargetCloseToAttack(model.Transform.position))
            {
                Attack(presenter, model.Damage);
                _animator.AnimateAttack();
            }
        }
    }
}