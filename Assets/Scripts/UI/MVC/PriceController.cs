using Code.MVC;
using System;

namespace Code.UI
{
    public sealed class PriceController : Controller<PricePanel, PriceModel>, IDisposable
    {
        public PriceController() : base() 
            => Model.OnTriggerBuilding += WorkConstruction;

        public void WorkConstruction(BuildActionType action)
        {
            if (action == BuildActionType.Show)
                Show();
            else
                Hide();
        }

        protected override void Show()
        {
            View.gameObject.SetActive(true);
            View.Price.text = Model.CurrentPrice.ToString();
        }

        public override void UpdateView() { }

        protected override void Hide(bool isDestroyed = false)
        {
            if (isDestroyed)
                Dispose();
            else if (View.isActiveAndEnabled)
                View.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            Model.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}