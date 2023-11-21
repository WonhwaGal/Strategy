using Code.Tools;

namespace Code.Strategy
{
    public class StrategyHandler : IService
    {
        public IStrategy GetStrategy(PrefabType type)
        {
            if(type == PrefabType.Player)
                return new PlayerStrategy();
            if ((int)type < Constants.PlayerThreshold)
                return new DayBuildingStrategy();
            else if ((int)type >= Constants.EnemyThreshold)
                return new EnemyInfantryStrategy();

            return null;
        }
    }
}