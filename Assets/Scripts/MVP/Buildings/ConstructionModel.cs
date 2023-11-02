using System;

public class ConstructionModel : IDisposable
{
    private readonly int _id;
    private readonly int _activatedBy;
    private readonly int _defense;

    public int ID => _id;
    public int ActivatedBy => _activatedBy;
    public bool AutoVisible { get; private set; }

    public event Action OnDestroyed;

    public ConstructionModel(SingleBuildingData data)
    {
        _id = data.ID;
        _activatedBy = data.ActivatedBy;
        _defense = data.Defense;
        AutoVisible = data.AutoVisible;
    }

    public void Dispose()
    {

    }
}