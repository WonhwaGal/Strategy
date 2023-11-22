using UnityEngine;
using Code.Combat;
using Code.Units;
using Code.Construction;

namespace Code.Strategy
{
    public abstract class BaseInfantryStrategy : IUnitStrategy
    {
        protected readonly CombatService _combatService;
        protected Transform _target;
        protected float _targetRadius;

        public BaseInfantryStrategy(IUnitPresenter presenter = null)
        {
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
            if (presenter != null)
                Init(presenter);
        }

        public void Init(IUnitPresenter presenter) => presenter.View.NavAgent.speed = presenter.Model.Speed;

        protected virtual void ReceiveTarget(IModel newTarget)
        {
            _target = newTarget.Transform;
            _targetRadius = newTarget.DamageRadius;
        }

        public virtual void Execute(IUnitPresenter presenter, float delta) { }

        public virtual void SwitchStrategy(IUnitPresenter presenter, GameMode mode) { }

        protected abstract void Move(UnitView view, UnitModel model, float delta);

        protected abstract void Await(UnitModel model, float delta);

        protected void SearchForNewOpponent(UnitModel model, bool onlyCastle = false)
        {
            var newTargetInfo = _combatService.ReceiveClosestTarget(model, onlyCastle);
            IPresenter presenter = newTargetInfo.Item1;

            if (presenter != null)
                OnFindTarget(model, presenter, newTargetInfo.Item2);
        }

        protected virtual void OnFindTarget(UnitModel model, IPresenter presenter, bool isUnit) { }

        protected bool CheckAttackConditions(Vector3 selfPos)
        {
            var distance = (_target.position - selfPos).magnitude;
            return distance <= _targetRadius;
        }

        protected void Attack(IPresenter target, int damage)
            => _combatService.HitTarget(target, damage);
    }
}