using Code.ScriptableObjects;
using System;

public class ConstructionModel : IDisposable
{
    private readonly int _id;
    private readonly int _activatedBy;
    private int _defense;

    public int ID => _id;
    public int ActivatedBy => _activatedBy;
    public int Defense
    {
        get => _defense;
        set
        {
            _defense = value;
            if(_defense <= 0)
                OnDestroyed?.Invoke();
        }
    }
    public bool AutoVisible { get; private set; }

    public event Action OnDestroyed;

    public ConstructionModel(SingleBuildingData data)
    {
        _id = data.ID;
        _activatedBy = data.ActivatedBy;
        _defense = data.CommonData.Defense;
        AutoVisible = data.AutoVisible;
    }

    public void Dispose()
    {

    }
}