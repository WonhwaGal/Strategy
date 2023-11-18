using Code.Construction;

namespace Code.Strategy
{
    public interface IConstructionStrategy : IStrategy
    {
        void Init(IConstructionPresenter presenter);
        void Execute(IConstructionPresenter presenter, float delta);
        void SwitchStrategy(IConstructionPresenter presenter, GameMode mode);
    }
}