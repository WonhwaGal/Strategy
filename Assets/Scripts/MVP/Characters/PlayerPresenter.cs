

namespace Code.Units
{
    public class PlayerPresenter : UnitPresenter
    {
        public PlayerPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy) 
            : base(view, model, moveStrategy)
        {
            _view.OnUpdate += Update;
        }

        private void Update() => _strategy.Execute(_model, _view);

        public override void Dispose()
        {
            _view.OnUpdate -= Update;
            base.Dispose();
        }
    }
}