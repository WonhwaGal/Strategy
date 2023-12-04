using Code.Units;
using UnityEngine;

namespace Code.Strategy
{
    public class DayAllyStrategy : BaseAllyDayStrategy
    {
        private Vector3 _orderedPosition;
        private const float PermittedShift = 2.0f;
        private const float MinShift = 1.0f;
        private const float MaxShift = 2.0f;
        private Vector3 _randomShift;
        private bool _shouldReturn;

        public DayAllyStrategy(IUnitPresenter presenter = null) : base(presenter) { }

        public override void Execute(IUnitPresenter presenter, float delta)
        {
            Move(presenter);
            _animator.AnimateMovement(presenter.View.NavAgent.hasPath);
        }

        public override void Init(IUnitPresenter presenter)
        {
            base.Init(presenter);

            var allyView = ((AllyUnit)presenter.View);
            _orderedPosition = allyView.OrderedPosition;
            _isUnderControl = allyView.UnderPlayerControl;

            if (_isUnderControl)
            {
                _leader = allyView.Leader;
                var randomValue = Random.Range(MinShift, MaxShift);
                _randomShift = new (randomValue, 0, randomValue);
            }
            IsLocatedFar(allyView);
        }

        public override void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            base.SwitchStrategy(presenter, mode);
            if (mode == GameMode.IsUnitControl)
                presenter.Strategy = new UnitControlStrategy(presenter);
            else if(mode == GameMode.IsDay)
                presenter.View.GameObject.SetActive(true);
        }

        private bool IsLocatedFar(UnitView view)
        {
            _shouldReturn = false;
            if ((view.transform.position - _orderedPosition).magnitude > PermittedShift)
                _shouldReturn = true;

            return _shouldReturn;
        }

        private void Move(IUnitPresenter presenter)
        {
            var agent = presenter.View.NavAgent;
            if (_isUnderControl)
            {
                agent.SetDestination(_leader.position + _randomShift);
            }
            else if (_shouldReturn)
            {
                IsLocatedFar(presenter.View);
                agent.SetDestination(_orderedPosition);
            }
        }
    }
}