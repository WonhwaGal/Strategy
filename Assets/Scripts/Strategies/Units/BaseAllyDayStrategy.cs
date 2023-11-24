using Code.Units;
using UnityEngine;

namespace Code.Strategy
{
    public abstract class BaseAllyDayStrategy : IUnitStrategy
    {
        protected Transform _leader;
        protected bool _isUnderControl;

        public abstract void Execute(IUnitPresenter presenter, float delta);

        public virtual void Init(IUnitPresenter presenter)
        {
            var allyView = ((AllyUnit)presenter.View);
            allyView.GameObject.SetActive(true);
            allyView.NavAgent.speed = presenter.Model.Speed;
        }


        public virtual void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            var type = presenter.Model.PrefabType;
            presenter.View.GameObject.SetActive(true);
            if (mode == GameMode.IsNight)
            {
                WaveLocator.ParticipateInCombat(type, presenter.View.gameObject, presenter);
                presenter.Strategy = new AllyInfantryStrategy(presenter);
                presenter.SetUpHPBar(UIType.AllyHP);
            }
        }

        protected void GetUnderControl(Transform leader)
        {
            _leader = leader;
            _isUnderControl = true;
        }
    }
}