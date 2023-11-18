using Code.Strategy;

namespace Code.Construction
{
    public interface IConstructionPresenter : IPresenter
    {
        ConstructionView View { get; }
        ConstructionModel Model { get; }
        IStrategy Strategy { get; set; } }
    }
