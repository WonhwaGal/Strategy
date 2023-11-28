using Code.Strategy;
using Code.UI;

namespace Code.Units
{
    public class PlayerPresenter : UnitPresenter
    {
        private RechargePanel _rechargePanel;
        
        public PlayerPresenter(UnitView view, UnitModel model, IUnitStrategy strategy)
            : base(view, model, strategy)
        { }

        public RechargePanel SetUpRechargePanel()
        {
            if (_rechargePanel == null)
                CreateRechargePanel();

            _rechargePanel.SetUpPanel(_model.AttackInterval * 3, _model.Transform);
            return _rechargePanel;
        }

        private void CreateRechargePanel()
        {
            var pool = ServiceLocator.Container.RequestFor<FollowUIPool>();
            _rechargePanel = (RechargePanel)pool.Spawn(UIType.PlayerRecharge);
            pool.OnSpawned(_rechargePanel);
        }

        public override void Dispose()
        {
            (_strategy as PlayerStrategy).Dispose();
            base.Dispose();
        }
    }
}