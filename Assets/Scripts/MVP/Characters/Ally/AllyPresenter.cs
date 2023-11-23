using Code.Strategy;

namespace Code.Units
{
    public class AllyPresenter : UnitPresenter
    {
        public AllyPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy) :
            base(view, model, moveStrategy)
        {
        }
    }
}