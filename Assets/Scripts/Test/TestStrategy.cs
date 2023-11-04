using UnityEngine;

namespace Code.Strategy
{
    public class TestStrategy : IConstructionStrategy
    {
        public void Execute(ConstructionModel model)
        {
            Debug.Log("executing strategy");
        }

        public object Clone() => new TestStrategy();
    }
}