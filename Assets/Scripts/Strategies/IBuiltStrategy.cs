using System;

namespace Code.Strategy
{
    public interface IConstructionStrategy : ICloneable
    {
        void Execute(ConstructionModel model);
    }
}