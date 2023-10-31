using System;

public class ConstructionModel : IDisposable
{
    public event Action OnDestroyed;
    private readonly int QueueOrder;
    private readonly int Defense;

    public ConstructionModel(int queueOrder, int defense)
    {
        QueueOrder = queueOrder;
        Defense = defense;
    }

    public void Dispose()
    {

    }
}