using Code.Tools;

namespace Code.Strategy
{
    public static class StrategyHandler
    {
        public static IStrategy GetStrategy(PrefabType type)
        {
            if ((int)type < Constants.PlayerThreshold)
                return new DayBuildingStrategy();
            else if ((int)type >= Constants.EnemyThreshold)
                return new EnemyInfantryStrategy();

            if (type == PrefabType.Player)
                return new PlayerStrategy();
            else
                return new DayAllyStrategy();
        }
    }
}