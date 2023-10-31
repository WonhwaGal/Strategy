using UnityEngine;

namespace Code.Units
{
    public interface IUnitStrategy
    {
        void Execute(UnitModel model, UnitView view);
    }
}