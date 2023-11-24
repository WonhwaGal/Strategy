using Code.Units;
using UnityEngine;

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
            if (_isStatic)
                presenter.View.transform.LookAt(_target);
            else
                Move(presenter.View, presenter.Model, delta);

            Await(presenter.Model, delta);
        }

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