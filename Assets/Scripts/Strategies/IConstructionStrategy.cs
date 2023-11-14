using Code.Construction;

namespace Code.Strategy
{
    public interface IConstructionStrategy : IStrategy
    {
        void Execute(ConstructionModel model, float delta);
    }
}