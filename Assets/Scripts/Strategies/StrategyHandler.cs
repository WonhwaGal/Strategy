
namespace Code.Strategy
{
    public class StrategyHandler : IService
    {
        public IStrategy GetStrategy(PrefabType type)
        {
            IStrategy strategy = type switch
            {
                PrefabType.Castle => new CombatBuildingStrategy(),
                PrefabType.Tower => new CombatBuildingStrategy(),
                PrefabType.Player => new PlayerStrategy(),
                PrefabType.Enemy => new InfantryStrategy(),
                _ => new PassiveStrategy(),
            };
            return strategy;
        }
    }
}