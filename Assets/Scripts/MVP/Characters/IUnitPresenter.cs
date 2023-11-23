using Code.Strategy;

namespace Code.Units
{
    public interface IUnitPresenter : IPresenter
    {
        UnitView View { get; }
        UnitModel Model { get; }
        IStrategy Strategy { get; set; }
        void SetUpHPBar(UIType uiType);
    }
}