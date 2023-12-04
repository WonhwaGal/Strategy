using System;
using Code.Construction;
using UnityEngine;

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
        public event Action<bool> OnCancelUpgrade;

        private void HandleConstruction(IConstructionModel model, BuildActionType action)
        {
            if (action == BuildActionType.Upgrade)
            {
                CurrentModel = model;
                //fill panel with options
                OnUpgradeBuilding?.Invoke();
            }
            else if(action == BuildActionType.Hide)
            {
                OnCancelUpgrade?.Invoke(false);
            }
        }

        public void Update() => _service.Upgrade();

        public void Dispose() => _service.OnNotifyConnections -= HandleConstruction;
    }
}