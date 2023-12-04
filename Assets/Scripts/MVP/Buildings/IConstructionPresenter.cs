using Code.Strategy;
using Code.UI;

namespace Code.Construction
{
    public interface IConstructionPresenter : IPresenter
    {
        bool IsResponsive { get; set; }
        ConstructionView View { get; }
        ConstructionModel Model { get; }
        IStrategy Strategy { get; set; }
        HPBar HPBar { get; }
        void SetUpHPBar();
    }
}