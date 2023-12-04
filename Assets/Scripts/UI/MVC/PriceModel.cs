using System;
using Code.Construction;
using Code.MVC;

namespace Code.UI
{
    public class PriceModel : IUiModel, IDisposable
    {
        public PriceModel()
        {
            ServiceLocator.Container.RequestFor<ConstructionService>()
                .OnNotifyConnections += HandleConstruction;
        }
        public int CurrentPrice { get; private set; }
        public string CurrentType { get; private set; }

        public event Action<BuildActionType> OnTriggerBuilding;

        private void HandleConstruction(IConstructionModel model, BuildActionType action)
        {
            if (action == BuildActionType.Show)
            {
                CurrentPrice = model.PriceList[model.CurrentStage];
                CurrentType = model.PrefabType.ToString();
            }
            else if (action == BuildActionType.Hide)
            {
                CurrentPrice = 0;
                CurrentType = "";
            }

            OnTriggerBuilding?.Invoke(action);
        }

        public void Dispose() 
        {
            ServiceLocator.Container.RequestFor<ConstructionService>()
                .OnNotifyConnections -= HandleConstruction;
        }
    }
}