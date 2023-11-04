using Code.ScriptableObjects;
using System;

public class ConstructionModel : IDisposable
{
    private int _id;
    private int _activatedBy;
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
        _id = data.UniqueInfo.ID;
        _activatedBy = data.UniqueInfo.ActivatedBy;
        _defense = data.CommonInfo.Defense;
        AutoVisible = data.UniqueInfo.AutoVisible;
    }

    public void AssignUniqueValues(int id, int activatedby, bool isAutoVis)
    {
        _id = id;
        _activatedBy = activatedby;
        AutoVisible = isAutoVis;
    }

    public ConstructionModel Clone(int id, int actBy, bool auto)
    {
        var model = this.MemberwiseClone() as ConstructionModel;
        model.AssignUniqueValues(id, actBy, auto);
        return model;
    }

    public void Dispose()
    {

    }
}