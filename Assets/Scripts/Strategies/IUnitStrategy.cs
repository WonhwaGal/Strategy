using System;
using Code.Units;

namespace Code.Strategy
{
    public interface IUnitStrategy : IStrategy, IDisposable
    {
        void Execute(UnitModel model, UnitView view, float delta);
    }
}