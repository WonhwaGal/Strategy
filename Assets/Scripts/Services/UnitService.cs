using Code.ScriptableObjects;
using Code.Pools;
using static WaveSO;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Code.Units
{
    public class UnitService : IService
    {
        private readonly UnitSettingList _unitSetList;
        private readonly UnitCreator _unitCreator;

        public UnitService(UnitSettingList unitSetList, UnitPrefabs prefabs)
        {
            _unitSetList = unitSetList;
            _unitCreator = new UnitCreator(new UnitMultiPool(prefabs));
        }

        public UnitPresenter CreateUnit(PrefabType type)
        {
            UnitPresenter presenter = _unitCreator.CreatePresenter(type, _unitSetList.FindUnit(type)); 
            presenter.OnBeingKilled += DestroyPresenter;
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
                SpawnAlly(type, spawnPoints[i].position, true);
        }
        
        private AllyPresenter SpawnAlly(PrefabType type, Vector3 position, bool active)
        {
            var presenter = CreateUnit(type);
            presenter.PlaceUnit(position);
            ((IUnitPresenter)presenter).View.gameObject.SetActive(active);
            return (AllyPresenter)presenter;
        }

        public void DestroyPresenter(IPresenter presenter, IUnitView view, bool destroyView)
        {
            if (view != null && !destroyView)
            {
                _unitCreator.Despawn(view.PrefabType, (UnitView)view);
                if (view.PrefabType == PrefabType.Ally)
                    SpawnAlly(view.PrefabType, ((AllyUnit)view).OrderedPosition, false);
            }

            presenter.OnBeingKilled -= DestroyPresenter;
            presenter.Dispose();
        }
    }
}