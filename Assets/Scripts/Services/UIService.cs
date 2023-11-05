using Code.Construction;
using System;

namespace Code.UI
{
    public class UIService : IService
    {
        private readonly UpgradePanel _upgradePanel;
        private readonly PricePanel _pricePanel;

        public event Action OnChooseUpgrade;

        public UIService(UpgradePanel panel, PricePanel pricePanel)  // REDO EVERYTHING
        {
            _upgradePanel = panel;
            _pricePanel = pricePanel;
            _upgradePanel.gameObject.SetActive(false);
            _pricePanel.gameObject.SetActive(false);
            _upgradePanel.BuyButton.onClick.AddListener(() =>
            {
                _upgradePanel.gameObject.SetActive(false);
                OnChooseUpgrade?.Invoke();
            });
        }

        public void ShowPanel(IConstructionModel model, BuildActionType action)
        {
            _pricePanel.gameObject.SetActive(false);
            _upgradePanel.gameObject.SetActive(false);

            switch (action)
            {
                case BuildActionType.Upgrade:
                    _upgradePanel.gameObject.SetActive(true);
                    //fill panel with options
                    break;
                case BuildActionType.Build:
                    return;
                default:
                    if (model.CurrentStage >= model.TotalStages - 1)
                        return;
                    _pricePanel.gameObject.SetActive(action == BuildActionType.Show);
                    _pricePanel.Price.text = model.PriceList[model.CurrentStage].ToString();
                    break;
            }
        }
    }
}