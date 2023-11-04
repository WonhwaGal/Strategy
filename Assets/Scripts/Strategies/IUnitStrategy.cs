using Code.Units;


namespace Code.Strategy
{
    public interface IUnitStrategy
    {
        void Execute(UnitModel model, UnitView view);
    }
}