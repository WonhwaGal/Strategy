using Code.Units;
using UnityEngine;

namespace Code.Strategy
{
    public abstract class BaseAllyDayStrategy : IUnitStrategy
    {
        protected Transform _leader;
        protected bool _isUnderControl;
        protected UnitAnimator _animator;

        public BaseAllyDayStrategy(IUnitPresenter presenter = null)
        {
            if (presenter != null)
                Init(presenter);
        }

        public abstract void Execute(IUnitPresenter presenter, float delta);

        public virtual void Init(IUnitPresenter presenter)
        {
            var allyView = ((AllyUnit)presenter.View);
            allyView.GameObject.SetActive(true);

            allyView.NavAgent.speed = presenter.Model.Speed;
            _animator = new UnitAnimator(allyView.Animator);
        }


        public virtual void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsNight)
                presenter.Strategy = new AllyInfantryStrategy(presenter);
        }
    }
}