using Code.Construction;
using Code.UI;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    [SerializeField] private UpgradePanel _upgradePanel;
    [SerializeField] private PricePanel _pricePanel;
    private UIService _uiService;
    private ConstructionService _constrService;

    void Start()
    {
        _uiService = new UIService(_upgradePanel, _pricePanel);
        _constrService = ServiceLocator.Container.RequestFor<ConstructionService>();
        _constrService.OnNotifyConnections += _uiService.ShowPanel;
        _uiService.OnChooseUpgrade += _constrService.Upgrade;
    }

    private void OnDestroy()
    {
        _constrService.OnNotifyConnections -= _uiService.ShowPanel;
        _uiService.OnChooseUpgrade -= _constrService.Upgrade;
    }
}