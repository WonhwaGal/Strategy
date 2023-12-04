using Code.Strategy;

namespace Code.Units
{
    public class EnemyPresenter : UnitPresenter
    {
        public EnemyPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy) :
            base(view, model, moveStrategy)
        {
        }
    }
}