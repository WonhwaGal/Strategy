using Code.Strategy;
using Code.UI;

namespace Code.Construction
{
    public interface IConstructionPresenter : IPresenter
    {
        ConstructionView View { get; }
        ConstructionModel Model { get; }
        IStrategy Strategy { get; set; }
        HPBar HPBar { get; }
        void SetUpHPBar(UIType uiType);
    }
}