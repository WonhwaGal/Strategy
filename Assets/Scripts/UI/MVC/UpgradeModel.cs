using System;
using Code.Construction;

namespace Code.MVC
{
    public sealed class UpgradeModel : IUiModel, IDisposable
    {
        private readonly ConstructionService _service;
        public UpgradeModel()
        {
            _service = ServiceLocator.Container.RequestFor<ConstructionService>();
            _service.OnNotifyConnections += HandleConstruction;
        }
        public IConstructionModel CurrentModel { get; private set; }

        public event Action OnUpgradeBuilding;

        private void HandleConstruction(IConstructionModel model, BuildActionType action)
        {
            if (action != BuildActionType.Upgrade)
                return;
            CurrentModel = model;
            //fill panel with options

            OnUpgradeBuilding?.Invoke();
        }

        public void Update() => _service.Upgrade();

        public void Dispose() => _service.OnNotifyConnections -= HandleConstruction;
    }
}