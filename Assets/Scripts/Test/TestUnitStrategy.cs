using Code.Units;
using UnityEngine;

namespace Code.Strategy
{
    public class TestUnitStrategy : IUnitStrategy
    {
        public void Execute(UnitModel model, UnitView view, float delta)
        {
            //Debug.Log("executing unit strategy");
        }
    }
}