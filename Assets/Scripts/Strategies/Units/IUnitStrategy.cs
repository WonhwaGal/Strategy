using Code.Units;

namespace Code.Strategy
{
    public interface IUnitStrategy : IStrategy
    {
        void Init(IUnitPresenter presenter);
        void Execute(IUnitPresenter presenter, float delta);
        void SwitchStrategy(IUnitPresenter presenter, GameMode mode);
    }
}