
namespace Code.Strategy
{
    public class StrategyHandler : IService
    {
        public IStrategy GetStrategy(PrefabType type)
        {
            IStrategy strategy = type switch
            {
                PrefabType.Castle => new TestStrategy(),
                PrefabType.Player => new PlayerMovementStrategy(),
                _ => new TestStrategy(),
            };
            return strategy;
        }
    }
}