using Code.Strategy;
using UnityEngine;

namespace Code.Units
{
    public class EnemyPresenter : UnitPresenter
    {
        private readonly CombatService _combatService;

        public EnemyPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy) : 
            base(view, model, moveStrategy)
        {
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
            _combatService.OnLocatingCollider += CheckCollider;
        }

        private void CheckCollider(GameObject obj)
        {
            if (_view.gameObject.Equals(obj))
                ReceiveDamage();
        }

        public override void Dispose()
        {
            base.Dispose();
            _combatService.OnLocatingCollider -= CheckCollider;
        }
    }
}