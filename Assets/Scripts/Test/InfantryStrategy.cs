using Code.Combat;
using Code.Units;
using UnityEngine;

namespace Code.Strategy
{
    public class InfantryStrategy : IUnitStrategy
    {
        private readonly CombatService _combatService;
        private Vector3 _targetPos;

        public InfantryStrategy()
        {
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
            AssignTarget(_combatService.Castle);
        }

        public bool IsNight { get; set; }

        public void Execute(UnitModel model, UnitView view, float delta)
        {
            Move(model, view);
        }

        private Vector3 AssignTarget(Vector3 target) => _targetPos = target;

        private void Move(UnitModel model, UnitView view)
        {
            if (model.HP > 0 && _targetPos != null)
                view.NavAgent.SetDestination(_targetPos);
        }

        public void Dispose()
        {
        }
    }
}