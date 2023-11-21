using Code.Strategy;
using Code.UI;

namespace Code.Units
{
    public class EnemyPresenter : UnitPresenter
    {
        public EnemyPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy, HPBar hpBar) :
            base(view, model, moveStrategy, hpBar)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}