using UnityEngine.UI;

namespace Code.UI
{
    public class UpgradePanel : UIView
    {
        public Button BuyButton;

        private void Start() => BuyButton.onClick.AddListener(ClosePanel);

        private void ClosePanel() => gameObject.SetActive(false);

        protected override void OnViewDestroy() => BuyButton.onClick.RemoveAllListeners();
    }
}