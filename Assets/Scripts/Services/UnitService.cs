using System;
using UnityEngine;
using Code.Units;
using Code.Construction;
using Code.ScriptableObjects;
using Code.Strategy;
using Code.Pools;
using static WaveSO;
using Random = UnityEngine.Random;

public class UnitService : ICombatService
{
    private readonly UnitSettingList _unitSetList;
    private readonly StrategyHandler _strategyHandler;
    private readonly MultiPool<PrefabType, UnitView> _unitPool;

    public UnitService(UnitSettingList unitSetList, UnitPrefabs prefabs)
    {
        _unitSetList = unitSetList;
        _unitPool = new UnitMultiPool(prefabs);
        _strategyHandler = ServiceLocator.Container.RequestFor<StrategyHandler>();
    }

    public event Action<GameMode> OnChangeGameStage;
    public event Action<PrefabType, GameObject, IPresenter> OnRegisterForCombat;

    public void SwitchMode(GameMode mode) => OnChangeGameStage?.Invoke(mode);

    public UnitPresenter CreateUnit(PrefabType type)
    {
        var view = _unitPool.Spawn(type);
        var model = new UnitModel(_unitSetList.FindUnit(type), view.transform);
        var strategy = (IUnitStrategy)_strategyHandler.GetStrategy(type);
        UnitPresenter presenter = type == PrefabType.Player ? 
            new PlayerPresenter(view, model, strategy) : new EnemyPresenter(view, model, strategy);
        presenter.OnBeingKilled += ResetPresenter;
        presenter.OnReadyForCombat += RegisterForCombat;
        presenter.OnRequestDestroy += DestroyPresenter;
        OnChangeGameStage += presenter.ChangeStage;
        return presenter;
    }

    public void CreateWave(LevelWaveData levelSpawns)
    {
        for (int i = 0; i < levelSpawns.SpawnSpots.Count; i++)
        {
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

    public void RegisterForCombat(PrefabType type, GameObject go, IPresenter presenter) 
        => OnRegisterForCombat?.Invoke(type, go, presenter);

    private void ResetPresenter(IPresenter presenter, IUnitView view) 
        => _unitPool.Despawn(view.PrefabType, (UnitView)view);

    public void DestroyPresenter(UnitPresenter presenter)
    {
        OnChangeGameStage -= presenter.ChangeStage;
        presenter.OnBeingKilled -= ResetPresenter;
        presenter.OnReadyForCombat -= RegisterForCombat;
        presenter.OnRequestDestroy -= DestroyPresenter;
        presenter.Dispose();
    }
}