
namespace Code.Strategy
{
    public class StrategyHandler : IService
    {
        public IStrategy GetStrategy(PrefabType type)
        {
            IStrategy strategy = type switch
            {
                PrefabType.Castle => new DayBuildingStrategy(),
                PrefabType.Tower => new DayBuildingStrategy(),
                PrefabType.Player => new PlayerStrategy(),
                PrefabType.Enemy => new EnemyStrategy(),
                _ => new PassiveStrategy(),
            };
            return strategy;
        }
    }
}