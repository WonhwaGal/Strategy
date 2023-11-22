using System;
using Code.Construction;
using Code.ScriptableObjects;
using Code.Pools;
using static WaveSO;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Code.Units
{
    public class UnitService : IReactToDaytimeSwitch
    {
        private readonly UnitSettingList _unitSetList;
        private readonly UnitCreator _unitCreator;

        public UnitService(UnitSettingList unitSetList, UnitPrefabs prefabs)
        {
            _unitSetList = unitSetList;
            _unitCreator = new UnitCreator(new UnitMultiPool(prefabs));
        }

        public event Action<GameMode> OnGameModeChange;

        public void SwitchMode(GameMode mode) => OnGameModeChange?.Invoke(mode);

        public UnitPresenter CreateUnit(PrefabType type)
        {
            UnitPresenter presenter = _unitCreator.CreatePresenter(type, _unitSetList.FindUnit(type)); 
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
                        Random.insideUnitSphere * spawnData.SpawnRadius;
                    presenter.PlaceUnit(spawnSpot);
                }
            }
        }

        public void SpawnAllies(Transform[] spawnPoints, PrefabType type)
        {
            for(int i = 0; i < spawnPoints.Length; i++)
            {
                var presenter = CreateUnit(type);
                presenter.PlaceUnit(spawnPoints[i].position);
            }
        }

        public void DestroyPresenter(IPresenter presenter, IUnitView view)
        {
            if (view != null)
                _unitCreator.Despawn(view.PrefabType, (UnitView)view);
            OnGameModeChange -= presenter.OnGameModeChange;
            presenter.OnBeingKilled -= DestroyPresenter;
            presenter.Dispose();
        }
    }
}