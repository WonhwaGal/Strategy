using Code.Construction;
using Code.Input;
using Code.ScriptableObjects;
using Code.UI;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    [SerializeField] private ConstructionSO _constructionSO;
    [SerializeField] private ConstructionPrefabs _buildingPrefabs;
    [SerializeField] private UnitPrefabs _unitPrefabs;
    [SerializeField] private UpgradePanel _upgradePanel;

    private ConstructionService _constructionService;
    private UnitService _unitService;
    private UIService _uiService;
    private IInputService _input;

    void Start()
    {
        ServiceLocator.Container.Register<IInputService>(new KeyboardInput());

        _constructionService = new ConstructionService(_constructionSO, _buildingPrefabs);
        _constructionService.StartLevel(1);

        _unitService = new UnitService(_unitPrefabs);
        _unitService.CreatePlayer();

        _uiService = new UIService(_upgradePanel);
        _constructionService.OnBuildConstruction += _uiService.PlaceUpgradePanel;

        _input = ServiceLocator.Container.RequestFor<IInputService>();
        _input.OnPressSpace += _constructionService.BuildConstruction;
    }
}