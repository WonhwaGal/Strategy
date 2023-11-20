using System;
using UnityEngine;
using Code.Units;
using Code.Construction;
using Code.ScriptableObjects;
using Code.Strategy;
using Code.Pools;
using static WaveSO;
using Random = UnityEngine.Random;

public class UnitService : IReactToDaytimeSwitch
{
    private readonly UnitSettingList _unitSetList;
    private readonly StrategyHandler _strategyHandler;
    private readonly UnitMultiPool _unitPool;

    public UnitService(UnitSettingList unitSetList, UnitPrefabs prefabs)
    {
        _unitSetList = unitSetList;
        _unitPool = new UnitMultiPool(prefabs);
        _strategyHandler = ServiceLocator.Container.RequestFor<StrategyHandler>();
    }

    public event Action<GameMode> OnGameModeChange;

    public void SwitchMode(GameMode mode) => OnGameModeChange?.Invoke(mode);

    public UnitPresenter CreateUnit(PrefabType type)
    {
        var view = _unitPool.Spawn(type);
        var model = new UnitModel(_unitSetList.FindUnit(type), view.transform);
        var strategy = (IUnitStrategy)_strategyHandler.GetStrategy(type);
        UnitPresenter presenter = type == PrefabType.Player ?
            new PlayerPresenter(view, model, strategy) : new EnemyPresenter(view, model, strategy);
        strategy.Init(presenter);
        presenter.OnBeingKilled += DestroyPresenter;
        OnGameModeChange += presenter.OnGameModeChange;
        return presenter;
    }

    public void CreateWave(LevelWaveData levelSpawns, int waveTurn)
    {
        for (int i = 0; i < levelSpawns.SpawnSpots.Count; i++)
        {
            if (levelSpawns.SpawnSpots[i].WaveTurn != waveTurn)
                continue;

            var spawnData = levelSpawns.SpawnSpots[i];
            for (int j = 0; j < spawnData.EnemyQuantity; j++)
            {
                var presenter = CreateUnit(spawnData.EnemyType);
                var spawnSpot = spawnData.SpawnCenter.position +
                    (Vector3)Random.insideUnitSphere * spawnData.SpawnRadius;
                presenter.PlaceUnit(spawnSpot);
            }
        }
    }

    public void DestroyPresenter(IPresenter presenter, IUnitView view)
    {
        if (view != null)
            _unitPool.Despawn(view.PrefabType, (UnitView)view);
        OnGameModeChange -= presenter.OnGameModeChange;
        presenter.OnBeingKilled -= DestroyPresenter;
        presenter.Dispose();
    }
}