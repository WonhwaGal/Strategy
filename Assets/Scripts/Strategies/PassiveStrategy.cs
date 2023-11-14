using Code.Construction;


namespace Code.Strategy
{
    public class PassiveStrategy : IConstructionStrategy
    {
        public bool IsNight { get; set; }

        public void Execute(ConstructionModel model, float delta) { }
    }
}