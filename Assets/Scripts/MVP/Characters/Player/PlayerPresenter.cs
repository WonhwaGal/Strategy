using Code.Strategy;

namespace Code.Units
{
    public class PlayerPresenter : UnitPresenter
    {
        public PlayerPresenter(UnitView view, UnitModel model, IUnitStrategy strategy) 
            : base(view, model, strategy) 
        {
        }

        protected override void OnStartNight(GameMode stage)
        {
            if (_strategy is PlayerStrategy playerStrategy)
                playerStrategy.IsNight = stage == GameMode.IsNight;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}