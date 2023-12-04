using Code.Construction;
using Code.Units;

namespace Code.Strategy
{
    public sealed class EnemyInfantryStrategy : BaseInfantryStrategy
    {
        private bool _isTargetingCastle;

        public EnemyInfantryStrategy(IUnitPresenter presenter = null) : base(presenter)
            => ReceiveTarget(_combatService.Castle);

        protected override void ReceiveTarget(IModel newTarget)
        {
            base.ReceiveTarget(newTarget);
            _isTargetingCastle = newTarget.PrefabType == PrefabType.Castle;
        }

        public override void Execute(IUnitPresenter presenter, float delta)
        {
            if (_stopActions)
            {
                presenter.View.NavAgent.isStopped = true;
                return;
            }

            Move(presenter.View, presenter.Model, delta);
        }

        protected override void Move(UnitView view, UnitModel model, float delta)
        {
            if (_target == null)
                ReceiveTarget(_combatService.Castle);

            if (!IsTargetCloseToAttack(view.transform.position))
                view.NavAgent.SetDestination(_target.position);
            else
                view.NavAgent.destination = view.transform.position;

            _animator.AnimateMovement(view.NavAgent.hasPath);
            Await(model, delta);
        }

        protected override void Await(UnitModel model, float delta)
        {
            _currentInterval += delta;
            if (_currentInterval >= model.AttackInterval)
            {
                if (_isTargetingCastle && IsTargetCloseToAttack(model.Transform.position))
                    SearchForNewOpponent(model, onlyCastle: true);
                else
                    SearchForNewOpponent(model);
                _currentInterval = 0;
            }
        }

        protected override void OnFindTarget(UnitModel model, IPresenter presenter, bool isUnit)
        {
            IModel targetModel = isUnit ?
                ((IUnitPresenter)presenter).Model : ((IConstructionPresenter)presenter).Model;

            ReceiveTarget(targetModel);
            if (IsTargetCloseToAttack(model.Transform.position))
            {
                Attack(presenter, model.Damage);
                _animator.AnimateAttack();
            }
        }
    }
}