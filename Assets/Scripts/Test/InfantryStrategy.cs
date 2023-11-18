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
        }

        public bool IsNight { get; set; }

        private Vector3 AssignTarget(Vector3 target) => _targetPos = target;

        private void Move(UnitModel model, UnitView view)
        {
            if (model.HP > 0 && _targetPos != null)
                view.NavAgent.SetDestination(_targetPos);
        }

        public void Dispose()
        {
        }

        public void Init(IUnitPresenter presenter)
        {
        }

        public void Execute(IUnitPresenter presenter, float delta)
        {
        }

        public void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
        }
    }
}