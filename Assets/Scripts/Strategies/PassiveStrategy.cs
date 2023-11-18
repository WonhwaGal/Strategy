using Code.Construction;


namespace Code.Strategy
{
    public class PassiveStrategy : IConstructionStrategy
    {
        public void Execute(IConstructionPresenter presenter, float delta)
        {
        }

        public void Init(IConstructionPresenter presenter)
        {
        }

        public void SwitchStrategy(IConstructionPresenter presenter, GameMode mode)
        {
        }
    }
}