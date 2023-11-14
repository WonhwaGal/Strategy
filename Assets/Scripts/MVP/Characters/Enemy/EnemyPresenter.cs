using Code.Combat;
using Code.Strategy;

namespace Code.Units
{
    public class EnemyPresenter : UnitPresenter
    {
        public EnemyPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy) : 
            base(view, model, moveStrategy)
        {
            _view.NavAgent.speed = _model.Speed;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}