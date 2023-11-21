using System;
using Code.Construction;
using Code.ScriptableObjects;
using UnityEngine;

namespace Code.UI
{
    public class UIService : IService
    {
        private readonly HPBarPool _hpPool;
        private UpgradePanel _upgradePanel;
        private PricePanel _pricePanel;

        public event Action OnChooseUpgrade;

        public UIService(UnsortedUIList unsorted, HPBarList hpList)
        {
            var canvasPrefab = (GameCanvas)unsorted.FindPrefab(UIType.GameCanvas);
            var canvas = GameObject.Instantiate<GameCanvas>(canvasPrefab);
            SetUpPanels(canvas);
            _hpPool = new HPBarPool(hpList, canvas.PoolRoot);
        }

        private void SetUpPanels(GameCanvas canvas)
        {
            _upgradePanel = canvas.UpgradePanel;
            _pricePanel = canvas.PricePanel;
            _upgradePanel.gameObject.SetActive(false);
            _pricePanel.gameObject.SetActive(false);
            _upgradePanel.BuyButton.onClick.AddListener(() =>
            {
                _upgradePanel.gameObject.SetActive(false);
                OnChooseUpgrade?.Invoke();
            });
        }

        public void ShowPanel(IConstructionModel model, BuildActionType action)
        {
            _pricePanel.gameObject.SetActive(false);
            _upgradePanel.gameObject.SetActive(false);

            switch (action)
            {
                case BuildActionType.Upgrade:
                    _upgradePanel.gameObject.SetActive(true);
                    //fill panel with options
                    break;
                case BuildActionType.Build:
                    return;
                default:
                    if (model.CurrentStage >= model.TotalStages - 1)
                        return;
                    _pricePanel.gameObject.SetActive(action == BuildActionType.Show);
                    _pricePanel.Price.text = model.PriceList[model.CurrentStage].ToString();
                    break;
            }
        }

        public HPBar SpawnHPBar(UIType type)
        {
            var result = _hpPool.Spawn(type);
            _hpPool.OnSpawned(result);
            result.OnOwnerKilled += DespawnHPBar;
            return result;
        }

        public void DespawnHPBar(HPBar hpbar)
        {
            _hpPool.Despawn(hpbar.UIType, hpbar);
            hpbar.OnOwnerKilled -= DespawnHPBar;
        }
    }
}