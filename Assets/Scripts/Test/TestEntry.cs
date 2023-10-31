using Code.Construction;
using Code.ScriptableObjects;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    [SerializeField] private ConstructionSO _constructionSO;
    [SerializeField] private ConstructionPrefabs _prefabs;
    [SerializeField] private UnitPrefabs _unitPrefabs;

    private ConstructionService _service;
    private UnitService _unitService;
    void Start()
    {
        _service = new ConstructionService(_constructionSO, _prefabs);
        _service.StartLevel(1);
        _unitService = new UnitService(_unitPrefabs);
        _unitService.CreatePlayer();
    }

}