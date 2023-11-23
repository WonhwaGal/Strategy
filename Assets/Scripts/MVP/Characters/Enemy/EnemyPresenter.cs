using Code.Strategy;
using Code.UI;

namespace Code.Units
{
    public class EnemyPresenter : UnitPresenter
    {
        public EnemyPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy) :
            base(view, model, moveStrategy)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}