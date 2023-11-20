using UnityEngine;
using Code.Combat;
using Code.Units;
using Code.Construction;

namespace Code.Strategy
{
    public sealed class EnemyStrategy : IUnitStrategy
    {
        private Transform _target;
        private float _targetRadius;
        private float _currentInterval = 0;
        private readonly CombatService _combatService;

        public EnemyStrategy(IUnitPresenter presenter = null)
        {
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
            ReceiveTarget(_combatService.Castle);

            if (presenter != null)
                Init(presenter);
        }

        public void Init(IUnitPresenter presenter) => presenter.View.NavAgent.speed = presenter.Model.Speed;

        private void ReceiveTarget(IModel newTarget)
        {
            _target = newTarget.Transform;
            _targetRadius = newTarget.CloseRadius;
        }

        public void Execute(IUnitPresenter presenter, float delta)
            => Move(presenter.View, presenter.Model, delta);

        public void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            var type = presenter.Model.PrefabType;
            if (mode == GameMode.IsNight)
                WaveLocator.ParticipateInCombat(type, presenter.View.gameObject, presenter);
        }

        private void Move(UnitView view, UnitModel model, float delta)
        {
            if (_target == null)
                ReceiveTarget(_combatService.Castle);

            view.NavAgent.SetDestination(_target.position);
            Await(model, delta);
        }

        public void Await(UnitModel model, float delta)
        {
            _currentInterval += delta;
            if (_currentInterval >= model.AttackInterval)
            {
                SearchForNewOpponents(model);
                _currentInterval = 0;
            }
        }

        private void SearchForNewOpponents(UnitModel model)
        {
            var newTargetInfo = _combatService.ReceiveClosestTarget(model);
            IPresenter presenter = newTargetInfo.Item1;

            if (presenter != null)
            {
                IModel targetModel = newTargetInfo.Item2 ?
                    ((IUnitPresenter)presenter).Model : ((IConstructionPresenter)presenter).Model;
                ReceiveTarget(targetModel);
                CheckAttackConditions(model, presenter);
            }
        }

        private void CheckAttackConditions(UnitModel model, IPresenter presenter)
        {
            var distance = (_target.position - model.Transform.position).magnitude;
            if (distance <= _targetRadius)
                Attack(presenter, model.Damage);
        }

        private void Attack(IPresenter target, int damage)
            => _combatService.HitTarget(target, damage);
    }
}