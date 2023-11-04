
namespace Code.Strategy
{
    public class StrategHandler
    {
        public IConstructionStrategy GetStrategy(PrefabType type)
        {
            IConstructionStrategy strategy = type switch
            {
                PrefabType.Castle => new TestStrategy(),
                _ => new TestStrategy(),
            };
            return strategy;
        }
    }
}