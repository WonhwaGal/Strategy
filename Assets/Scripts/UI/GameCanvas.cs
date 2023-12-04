using UnityEngine;

namespace Code.UI
{
    public class GameCanvas : UIView
    {
        [SerializeField] private UpgradePanel _upgradePanel;
        [SerializeField] private PricePanel _pricePanel;
        [SerializeField] private UIPoolRoot _poolRoot;

        public UpgradePanel UpgradePanel => _upgradePanel;
        public PricePanel PricePanel => _pricePanel;
        public UIPoolRoot PoolRoot => _poolRoot;

        private void Start()
        {
            _upgradePanel.gameObject.SetActive(false);
            _pricePanel.gameObject.SetActive(false);
        }
    }
}