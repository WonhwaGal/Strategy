using Code.Units;
using Code.ScriptableObjects;
using UnityEngine;
using Code.Strategy;

public class UnitService : IService
{
    private readonly PrefabSO<UnitView> _unitPrefabs;
    private readonly StrategyHandler _strategyHandler;
    public UnitService(PrefabSO<UnitView> prefabs)
    {
        _unitPrefabs = prefabs;
        _strategyHandler = ServiceLocator.Container.RequestFor<StrategyHandler>();
    }

    public void CreatePlayer()
    {
        var playerView = GameObject.Instantiate(
            _unitPrefabs.FindPrefab(PrefabType.Player), Vector3.zero, Quaternion.identity);
        var model = new UnitModel(hp: 100, speed: playerView.Speed);
        var strategy = (IUnitStrategy)_strategyHandler.GetStrategy(PrefabType.Player);
        new PlayerPresenter(playerView, model, strategy);
    }
}