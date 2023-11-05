using Code.Units;


namespace Code.Strategy
{
    public interface IUnitStrategy : IStrategy
    {
        void Execute(UnitModel model, UnitView view);
    }
}