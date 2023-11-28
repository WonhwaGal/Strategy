using System;
using UnityEngine;
using Code.MVC;
using Code.ScriptableObjects;

namespace Code.UI
{
    public class UIService : IService, IDisposable
    {
        private readonly PriceController _priceController;
        private readonly UpgradeController _upgradeController;

        public UIService(UnsortedUIList unsorted)
        {
            GameCanvas = SetUpCanvas(unsorted);
            _priceController = new();
            ((IController)_priceController).AddView(GameCanvas.PricePanel);
            _upgradeController = new();
            ((IController)_upgradeController).AddView(GameCanvas.UpgradePanel);
        }

        public GameCanvas GameCanvas { get; private set; }

        public GameCanvas SetUpCanvas(UnsortedUIList unsorted)
        {
            var canvasPrefab = (GameCanvas)unsorted.FindPrefab(UIType.GameCanvas);
            return GameObject.Instantiate<GameCanvas>(canvasPrefab);
        }

        public void Dispose()
        {
            _priceController.Dispose();
            _upgradeController.Dispose();
        }
    }
}