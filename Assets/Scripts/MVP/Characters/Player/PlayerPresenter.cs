using Code.Strategy;
using Code.UI;

namespace Code.Units
{
    public class PlayerPresenter : UnitPresenter
    {
        public PlayerPresenter(UnitView view, UnitModel model, IUnitStrategy strategy, HPBar hpBar)
            : base(view, model, strategy, hpBar)
        { }

        public override void Dispose()
        {
            (_strategy as PlayerStrategy).Dispose();
            base.Dispose();
        }
    }
}