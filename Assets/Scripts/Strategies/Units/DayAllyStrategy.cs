using Code.Units;

namespace Code.Strategy
{
    public class DayAllyStrategy : IUnitStrategy
    {
        public DayAllyStrategy(IUnitPresenter presenter = null)
        {
            if (presenter != null)
                Init(presenter);
        }

        public void Execute(IUnitPresenter presenter, float delta) { }

        public void Init(IUnitPresenter presenter)
            => presenter.View.NavAgent.speed = presenter.Model.Speed;

        public void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            var type = presenter.Model.PrefabType;
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