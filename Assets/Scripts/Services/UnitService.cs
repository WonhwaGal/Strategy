using UnityEngine;
using Code.ScriptableObjects;
using Code.Pools;
using static WaveSO;
using Random = UnityEngine.Random;
using System.Threading.Tasks;

namespace Code.Units
{
    public class UnitService : IService
    {
        private readonly UnitSettingList _unitSetList;
        private readonly UnitCreator _unitCreator;
        private const int SpawnDelay = 350;

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

        public async void CreateWave(LevelWaveData levelSpawns, int waveTurn)
        {
            for (int i = 0; i < levelSpawns.SpawnSpots.Count; i++)
            {
                if (levelSpawns.SpawnSpots[i].WaveTurn != waveTurn)
                    continue;

                var spawnData = levelSpawns.SpawnSpots[i];

                for (int j = 0; j < spawnData.EnemyQuantity; j++)
                {
                    await Task.Delay(SpawnDelay);
                    var presenter = CreateUnit(spawnData.EnemyType);
                    var spawnSpot = spawnData.SpawnCenter.position +
                        Random.insideUnitSphere * spawnData.SpawnRadius;
                    spawnSpot = new Vector3(spawnSpot.x, spawnData.SpawnCenter.position.y, spawnSpot.z);
                    presenter.PlaceUnit(spawnSpot, spawnData.SpawnCenter.rotation);
                }
            }
        }

        public void SpawnAllies(Transform[] spawnPoints, PrefabType type)
        {
            for(int i = 0; i < spawnPoints.Length; i++)
                SpawnAlly(type, spawnPoints[i].position, spawnPoints[i].rotation, true);
        }
        
        private AllyPresenter SpawnAlly(PrefabType type, Vector3 pos, Quaternion rot, bool active)
        {
            var presenter = CreateUnit(type);
            presenter.PlaceUnit(pos, rot);
            ((IUnitPresenter)presenter).View.gameObject.SetActive(active);
            return (AllyPresenter)presenter;
        }

        public void DestroyPresenter(IPresenter presenter, IUnitView view, bool destroyView)
        {
            if (view != null && !destroyView)
            {
                _unitCreator.Despawn(view.PrefabType, (UnitView)view);
                if (view.PrefabType == PrefabType.Ally)
                    SpawnAlly(view.PrefabType, ((AllyUnit)view).OrderedPosition, Quaternion.identity, false);
            }

            presenter.OnBeingKilled -= DestroyPresenter;
            presenter.Dispose();
        }
    }
}