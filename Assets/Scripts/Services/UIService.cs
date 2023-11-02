using UnityEngine;

namespace Code.UI
{
    public class UIService : IService
    {
        private readonly UpgradePanel _panel;

        public UIService(UpgradePanel panel)  // REDO EVERYTHING
        {
            _panel = panel;
            _panel.gameObject.SetActive(false);
            _panel.BuyButton.onClick.AddListener(() => _panel.gameObject.SetActive(false));
        }

        public void PlaceUpgradePanel(Vector3 pos)
        {
            _panel.transform.position = pos;
            _panel.gameObject.SetActive(true);
        }
    }
}