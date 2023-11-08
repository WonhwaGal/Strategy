using Code.Strategy;

namespace Code.Units
{
    public class PlayerPresenter : UnitPresenter
    {
        public PlayerPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy) 
            : base(view, model, moveStrategy) 
        {}

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}