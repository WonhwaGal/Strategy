using UnityEngine;

public class TestStrategy : IConstructionStrategy
{
    public void Execute(ConstructionModel model)
    {
        Debug.Log("executing strategy");
    }
}