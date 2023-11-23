using Code.Units;
using UnityEngine;

namespace Code.Strategy
{
    public class DayAllyStrategy : IUnitStrategy
    {
        private Vector3 _orderedPosition;
        private const float PermittedShift = 0.5f;
        private bool _shouldReturn;

        public DayAllyStrategy(IUnitPresenter presenter = null)
        {
            if (presenter != null)
                Init(presenter);
        }

        public void Execute(IUnitPresenter presenter, float delta)
        {
            if (_shouldReturn)
                presenter.View.NavAgent.SetDestination(_orderedPosition);
        }

        public void Init(IUnitPresenter presenter)
        {
            var view = presenter.View;
            view.GameObject.SetActive(true);
            view.NavAgent.speed = presenter.Model.Speed;
            var allyView = ((AllyView)view);
            if (allyView.OrderedPosition != Vector3.zero)
                _orderedPosition = allyView.OrderedPosition;

            IsLocatedFar(view);
        }

        public void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            var type = presenter.Model.PrefabType;
            presenter.View.GameObject.SetActive(true);
            if (mode == GameMode.IsNight)
            {
                WaveLocator.ParticipateInCombat(type, presenter.View.gameObject, presenter);
                presenter.Strategy = new AllyInfantryStrategy(presenter);
                presenter.SetUpHPBar(UIType.AllyHP);
            }
            else if (mode == GameMode.IsUnitControl)
            {
                presenter.Strategy = new UnitControlStrategy(presenter);
            }
        }

        private bool IsLocatedFar(UnitView view)
        {
            _shouldReturn = false;
            if ((view.transform.position - _orderedPosition).magnitude > PermittedShift)
                _shouldReturn = true;

            return _shouldReturn;
        }
    }

    public sealed class UnitControlStrategy : IUnitStrategy
    {
        public UnitControlStrategy(IUnitPresenter presenter = null)
        {
            if (presenter != null)
                Init(presenter);
        }

        public void Execute(IUnitPresenter presenter, float delta) { }

        public void Init(IUnitPresenter presenter) { }

        public void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsNight)
            {
                WaveLocator.ParticipateInCombat(presenter.Model.PrefabType, presenter.View.gameObject, presenter);
                presenter.Strategy = new EnemyInfantryStrategy(presenter);
            }
            else if (mode == GameMode.IsDay)
            {
                presenter.Strategy = new DayAllyStrategy(presenter);
            }
        }
    }
}