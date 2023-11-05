using Code.Construction;
using System;

namespace Code.Strategy
{
    public interface IConstructionStrategy : IStrategy, ICloneable
    {
        void Execute(ConstructionModel model);
    }
}