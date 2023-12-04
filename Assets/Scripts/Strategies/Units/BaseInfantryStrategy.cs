using UnityEngine;
using Code.Combat;
using Code.Units;

namespace Code.Strategy
{
    public abstract class BaseInfantryStrategy : IUnitStrategy
    {
        protected readonly CombatService _combatService;
        protected Transform _target;
        protected float _targetRadius;
        protected float _currentInterval;
        protected UnitAnimator _animator;
        protected bool _stopActions;

        public BaseInfantryStrategy(IUnitPresenter presenter = null)
        {
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
            if (presenter != null)
                Init(presenter);
        }

        public void Init(IUnitPresenter presenter)
        {
            presenter.View.NavAgent.speed = presenter.Model.Speed;
            _currentInterval = Random.Range(0, presenter.Model.AttackInterval / 2);
            _animator = new UnitAnimator(presenter.View.Animator);
            presenter.Model.OnKilled += OnDying;
            _animator.OnDyingAnimated += ((UnitPresenter)presenter).Die;
            WaveLocator.ParticipateInCombat(presenter.Model.PrefabType, presenter.View.GameObject, presenter);
        }

        protected virtual void ReceiveTarget(IModel newTarget)
        {
            _target = newTarget.Transform;
            _targetRadius = newTarget.DamageRadius;
        }

        public abstract void Execute(IUnitPresenter presenter, float delta);

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

        protected bool IsTargetCloseToAttack(Vector3 selfPos)
        {
            var distance = (_target.position - selfPos).magnitude;
            return distance <= _targetRadius;
        }

        protected void Attack(IPresenter target, int damage)
            => _combatService.HitTarget(target, damage);

        private void OnDying()
        {
            _stopActions = true;
            _animator.AnimateDeath();
        }

        protected void Dispose() => _animator.Dispose();
    }
}