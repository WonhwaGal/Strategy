using Code.Units;
using UnityEngine;

namespace Code.Strategy
{
    public class DayAllyStrategy : BaseAllyDayStrategy
    {
        private Vector3 _orderedPosition;
        private const float PermittedShift = 0.5f;
        private bool _shouldReturn;

        public DayAllyStrategy(IUnitPresenter presenter = null)
        {
            if (presenter != null)
                Init(presenter);
        }

        public override void Execute(IUnitPresenter presenter, float delta)
        {
            if (_shouldReturn)
                presenter.View.NavAgent.SetDestination(_orderedPosition);

            if (_isUnderControl)
                presenter.View.NavAgent.SetDestination(_leader.position);
        }

        public override void Init(IUnitPresenter presenter)
        {
            base.Init(presenter);
            var allyView = ((AllyUnit)presenter.View);
            allyView.OnGetUnderControl += GetUnderControl;
            _isUnderControl = allyView.UnderPlayerControl;

            if (_isUnderControl)
                _leader = allyView.Leader;
            if (allyView.OrderedPosition != Vector3.zero)
                _orderedPosition = allyView.OrderedPosition;

            IsLocatedFar(allyView);
        }

        public override void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            base.SwitchStrategy(presenter, mode);

            if (mode != GameMode.IsDay)
                ((AllyUnit)presenter.View).OnGetUnderControl -= GetUnderControl;

            if (mode == GameMode.IsUnitControl)
                presenter.Strategy = new UnitControlStrategy(presenter);
        }

        private bool IsLocatedFar(UnitView view)
        {
            _shouldReturn = false;
            if ((view.transform.position - _orderedPosition).magnitude > PermittedShift)
                _shouldReturn = true;

            return _shouldReturn;
        }
    }
}