using Code.Units;
using Code.ScriptableObjects;
using UnityEngine;

public class UnitService : IService
{
    private readonly UnitPrefabs _prefabs;

    public UnitService(UnitPrefabs prefabs) => _prefabs = prefabs;

    public void CreatePlayer()
    {
        for (int i = 0; i < _prefabs.UnitsPrefabs.Count; i++)
        {
            UnitView playerView;
            if (_prefabs.UnitsPrefabs[i].Type == PrefabType.Player)
                playerView = GameObject.Instantiate(_prefabs.UnitsPrefabs[i].Prefab, Vector3.zero, Quaternion.identity);
            else
                return;

            var model = new UnitModel(hp: 100, speed: playerView.Speed);
            new PlayerPresenter(playerView, model, new PlayerMovementStrategy());
            SetUpCamera(playerView);
        }
    }

    private void SetUpCamera(UnitView playerView)
    {
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        camFollow.AssignTarget(playerView.transform);
    }
}