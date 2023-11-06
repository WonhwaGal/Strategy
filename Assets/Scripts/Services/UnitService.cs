using Code.Units;
using Code.ScriptableObjects;
using UnityEngine;
using Code.Strategy;

public class UnitService : IService
{
    private readonly UnitSettingList _unitSetList;
    private readonly PrefabSO<UnitView> _unitPrefabs;
    private readonly StrategyHandler _strategyHandler;

    public UnitService(UnitSettingList unitSetList, PrefabSO<UnitView> prefabs)
    {
        _unitSetList = unitSetList;
        _unitPrefabs = prefabs;
        _strategyHandler = ServiceLocator.Container.RequestFor<StrategyHandler>();
    }

    public void CreatePlayer()
    {
        var playerView = GameObject.Instantiate(
            _unitPrefabs.FindPrefab(PrefabType.Player), Vector3.zero, Quaternion.identity);
        var model = new UnitModel(_unitSetList.FindUnit(PrefabType.Player), playerView.transform);
        var strategy = (IUnitStrategy)_strategyHandler.GetStrategy(PrefabType.Player);
        new PlayerPresenter(playerView, model, strategy);
    }

    public void CreateTestUnits()
    {
        var view = GameObject.Instantiate(
            _unitPrefabs.FindPrefab(PrefabType.Enemy), new Vector3(28, 0.77f, 9), Quaternion.identity);
        var model = new UnitModel(_unitSetList.FindUnit(PrefabType.Enemy), view.transform);
        new UnitPresenter(view, model, new TestUnitStrategy());

        var view2 = GameObject.Instantiate(
            _unitPrefabs.FindPrefab(PrefabType.Enemy), new Vector3(8, 0.77f, 9), Quaternion.identity);
        var model2 = new UnitModel(_unitSetList.FindUnit(PrefabType.Enemy), view.transform);
        new UnitPresenter(view2, model2, new TestUnitStrategy());
    }
}