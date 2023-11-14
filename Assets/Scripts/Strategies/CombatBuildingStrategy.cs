using Code.Combat;
using Code.Construction;
using Code.Units;
using UnityEngine;

namespace Code.Strategy
{
    public class CombatBuildingStrategy : IConstructionStrategy
    {
        private readonly CombatService _combatService;
        private float _currentInterval = 0;

        public CombatBuildingStrategy()
        {
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
        }

        public bool IsNight { get; set; }

        public void Execute(ConstructionModel model, float delta)
        {
            if (IsNight)
                CountDown(model, delta);
        }

        public void AssignCatle(Vector3 pos) => _combatService.Castle = pos;

        private void CountDown(ConstructionModel model, float delta)
        {
            _currentInterval += delta;
            if (_currentInterval >= model.AttackInterval)
            {
                Attack(model, AttackType.Arrow);
                _currentInterval = 0;
            }
        }

        private void Attack(IModel model, AttackType attack) 
            => _combatService.CheckForTargets(model, attack);
    }
}