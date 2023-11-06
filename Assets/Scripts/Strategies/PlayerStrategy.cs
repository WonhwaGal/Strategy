using UnityEngine;
using Code.Input;
using Code.Units;

namespace Code.Strategy
{
    public class PlayerStrategy : IUnitStrategy
    {
        private readonly IInputService _input;
        private readonly CombatService _combatService;
        private Vector3 _targetPos;
        private float _currentInterval = 0;

        public PlayerStrategy()
        {
            _input = ServiceLocator.Container.RequestFor<IInputService>();
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
        }

        public void Execute(UnitModel model, UnitView view, float delta)
        {
            Move(model, view);
            CountDown(model, delta);
        }

        private void Move(UnitModel model, UnitView view)
        {
            _targetPos = _input.GetInput() * model.Speed + view.transform.position;
            if (model.HP > 0)
                view.NavAgent.SetDestination(_targetPos);
        }

        private void CountDown(UnitModel model, float delta)
        {
            _currentInterval += delta;
            if (_currentInterval >= model.AttackInterval)
            {
                Attack(model, AttackType.Arrow);
                _currentInterval = 0;
            }
        }

        private void Attack(IModel model, AttackType attack) =>
            _combatService.CheckForTargets(model, attack);
    }
}