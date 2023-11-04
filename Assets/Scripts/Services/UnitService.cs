using Code.Units;
using Code.ScriptableObjects;
using UnityEngine;

public class UnitService : IService
{
    private readonly PrefabSO<UnitView> _unitPrefabs;

    public UnitService(PrefabSO<UnitView> prefabs) => _unitPrefabs = prefabs;

    public void CreatePlayer()
    {
        var playerView = GameObject.Instantiate(
            _unitPrefabs.FindPrefab(PrefabType.Player), Vector3.zero, Quaternion.identity);
        var model = new UnitModel(hp: 100, speed: playerView.Speed);
        new PlayerPresenter(playerView, model, new PlayerMovementStrategy());
    }
}