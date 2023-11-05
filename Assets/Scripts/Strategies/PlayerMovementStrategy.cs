using UnityEngine;
using Code.Input;
using Code.Units;

namespace Code.Strategy
{
    public class PlayerMovementStrategy : IUnitStrategy
    {
        private readonly IInputService _input;
        private Vector3 _targetPos;

        public PlayerMovementStrategy() => _input = ServiceLocator.Container.RequestFor<IInputService>();

        public void Execute(UnitModel model, UnitView view)
        {
            Move(model, view);
        }

        private void Move(UnitModel model, UnitView view)
        {
            _targetPos = _input.GetInput() * model.Speed + view.transform.position;

            if (model.HP > 0)
                view.NavAgent.SetDestination(_targetPos);
        }
    }
}