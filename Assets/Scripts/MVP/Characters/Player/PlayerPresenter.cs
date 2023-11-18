using Code.Strategy;

namespace Code.Units
{
    public class PlayerPresenter : UnitPresenter
    {
        public PlayerPresenter(UnitView view, UnitModel model, IUnitStrategy strategy) 
            : base(view, model, strategy) 
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            (_strategy as PlayerStrategy).Dispose();
        }
    }
}