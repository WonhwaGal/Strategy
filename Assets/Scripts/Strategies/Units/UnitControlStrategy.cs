using UnityEngine;
using Code.Units;
using Code.Tools;

namespace Code.Strategy
{
    public sealed class UnitControlStrategy : BaseAllyDayStrategy
    {
        private bool _considerStatic;
        private float _currentInterval = 0;

        public UnitControlStrategy(IUnitPresenter presenter = null) : base(presenter) { }


        public override void Execute(IUnitPresenter presenter, float delta)
        {
            if (_isUnderControl)
                presenter.View.NavAgent.SetDestination(_leader.position);
            else if (_considerStatic)
                CountDownToStatic(presenter, delta);
            _animator.AnimateMovement(presenter.View.NavAgent.hasPath);
        }

        public override void Init(IUnitPresenter presenter)
        {
            base.Init(presenter);
            var allyView = ((AllyUnit)presenter.View);

            if (allyView.UnderPlayerControl)
            {
                _leader = null;
                _considerStatic = true;
                allyView.UnderPlayerControl = false;
                allyView.NavAgent.SetDestination(allyView.transform.position);
            }
            else
            {
                _leader = allyView.Leader;
                allyView.OnGetUnderControl += GetUnderControl;
                allyView.IsStaticInCombat = false;
            }
        }

        public override void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            base.SwitchStrategy(presenter, mode);

            if (mode != GameMode.IsUnitControl)
                ((AllyUnit)presenter.View).OnGetUnderControl -= GetUnderControl;

            if (mode == GameMode.IsDay)
            {
                ((AllyUnit)presenter.View).UnderPlayerControl = _isUnderControl;
                presenter.Strategy = new DayAllyStrategy(presenter);
            }
        }

        private void CountDownToStatic(IUnitPresenter presenter, float delta)
        {
            _currentInterval += delta;
            if (_currentInterval >= Constants.WaitUntilStatic)
            {
                ((AllyUnit)presenter.View).IsStaticInCombat = true;
                _considerStatic = false;
            }
        }

        private void GetUnderControl(Transform leader)
        {
            _leader = leader;
            _isUnderControl = true;
        }
    }
}