using System;
using Code.UI;

namespace Code.MVC
{
    public class UpgradeController : Controller<UpgradePanel, UpgradeModel>, IDisposable
    {
        private bool _viewIsAssigned;

        public UpgradeController() : base() => Model.OnUpgradeBuilding += Show;

        protected override void Show()
        {
            if (!_viewIsAssigned)
                AssignView();
            View.gameObject.SetActive(true);
        }

        public override void UpdateView() { }

        protected override void Hide(bool isDestroyed)
        {
            if(isDestroyed)
                Dispose();
        }

        private void AssignView()
        {
            View.BuyButton.onClick.AddListener(() => Model.Update());
            _viewIsAssigned = true;
        }

        public void Dispose()
        {
            Model.OnUpgradeBuilding -= Show;
            Model.Dispose();
        }
    }
}