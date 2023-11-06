using Code.Input;
using Code.Strategy;
using Code.Construction;
using Code.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.SceneLoaders
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private UnitSettingList _unitSetList;
        [SerializeField] private ConstructionSO _constructionSO;
        [SerializeField] private ConstructionPrefabs _buildingPrefabs;
        [SerializeField] private UnitPrefabs _unitPrefabs;

        private const string GameScene = "GameScene";

        private IInputService _input;
        private UnitService _unitService;
        private ConstructionService _constructionService;

        private void Awake() => DontDestroyOnLoad(this);
        private void Start() => LoadNextScene();

        private void LoadNextScene()
        {
            AddServices();
            SceneManager.LoadSceneAsync(GameScene);
            SceneManager.sceneLoaded += OnLoadEvent;
        }

        public void OnLoadEvent(Scene scene, LoadSceneMode mode)
        {
            _constructionService.StartLevel(1);
            _unitService.CreatePlayer();
            _unitService.CreateTestUnits();              // to delete
            SceneManager.sceneLoaded -= OnLoadEvent;
        }

        private void AddServices()
        {
            _input = ServiceLocator.Container.RegisterAndAssign<IInputService>(new KeyboardInput());
            ServiceLocator.Container.Register(new StrategyHandler());
            ServiceLocator.Container.RegisterAndAssign(new CombatService());
            _unitService = ServiceLocator.Container.RegisterAndAssign(new UnitService(_unitSetList, _unitPrefabs));
            _constructionService = ServiceLocator.Container.
                RegisterAndAssign(new ConstructionService(_constructionSO, _buildingPrefabs));
            _input.OnPressSpace += _constructionService.TryToBuild;
        }

        private void OnDestroy()
        {
            _input.OnPressSpace -= _constructionService.TryToBuild;
        }
    }
}