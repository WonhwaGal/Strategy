using Code.Strategy;
using Code.UI;

namespace Code.Units
{
    public class AllyPresenter : UnitPresenter
    {
        public AllyPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy, HPBar hpBar) :
            base(view, model, moveStrategy, hpBar)
        {
        }
    }
}