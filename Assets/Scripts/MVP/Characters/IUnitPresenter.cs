using Code.Strategy;
using Code.UI;

namespace Code.Units
{
    public interface IUnitPresenter : IPresenter
    {
        UnitView View { get; }
        UnitModel Model { get; }
        IStrategy Strategy { get; set; }
        HPBar HPBar { get; }
        void SetUpHPBar();
    }
}