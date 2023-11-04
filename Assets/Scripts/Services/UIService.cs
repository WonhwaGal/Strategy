using UnityEngine;

namespace Code.UI
{
    public class UIService : IService
    {
        private readonly UpgradePanel _panel;
        private readonly PricePanel _pricePanel;

        public UIService(UpgradePanel panel, PricePanel pricePanel)  // REDO EVERYTHING
        {
            _panel = panel;
            _pricePanel = pricePanel;
            _panel.gameObject.SetActive(false);
            pricePanel.gameObject.SetActive(false);
            _panel.BuyButton.onClick.AddListener(() => _panel.gameObject.SetActive(false));
        }

        public void PlaceUpgradePanel(Vector3 pos)
        {
            _panel.transform.position = pos;
            _panel.gameObject.SetActive(true);
        }
    }
}