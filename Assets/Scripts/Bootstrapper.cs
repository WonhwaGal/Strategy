using UnityEngine;
using UnityEngine.SceneManagement;
using Code.Input;
using Code.Strategy;
using Code.Construction;
using Code.ScriptableObjects;
using Code.Combat;
using Code.Pools;

namespace Code.SceneLoaders
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private UnitSettingList _unitSetList;
        [SerializeField] private ConstructionSO _constructionSO;
        [SerializeField] private ConstructionPrefabs _buildingPrefabs;
        [SerializeField] private UnitPrefabs _unitPrefabs;
        [SerializeField] private WeaponList _weaponList;
        [SerializeField] private WaveSO _wavesList;
        [SerializeField] private RuinList _ruinsList;

        private const string GameScene = "GameScene";

        private IInputService _input;
        private UnitService _unitService;
        private LevelService _levelService;
        private ConstructionService _constructionService;

        private void Awake() => DontDestroyOnLoad(this);
        private void Start() => LoadNextScene();

        private void LoadNextScene()
        {
            SceneManager.LoadSceneAsync(GameScene);
            SceneManager.sceneLoaded += OnLoadEvent;
        }

        public void OnLoadEvent(Scene scene, LoadSceneMode mode)
        {
            AddServices();
            _constructionService.StartLevel(1);
            _unitService.CreateUnit(PrefabType.Player);
            SceneManager.sceneLoaded -= OnLoadEvent;
        }

        private void AddServices()
        {
            _input = ServiceLocator.Container.RegisterAndAssign<IInputService>(new KeyboardInput());
            ServiceLocator.Container.Register(new StrategyHandler());
            ServiceLocator.Container.Register(new CombatService(_weaponList));
            ServiceLocator.Container.Register(new RuinMultiPool(_ruinsList));
            _unitService = ServiceLocator.Container.RegisterAndAssign(new UnitService(_unitSetList, _unitPrefabs));
            _constructionService = ServiceLocator.Container.
                RegisterAndAssign(new ConstructionService(_constructionSO, _buildingPrefabs));
            _levelService = ServiceLocator.Container.RegisterAndAssign(new LevelService(_wavesList));
            Subsribe();
        }

        private void Subsribe()
        {
            _input.OnPressSpace += _levelService.OnSpacePressed;
            _levelService.OnCreatingWaves += _unitService.CreateWave;
            _levelService.OnChangingGameMode += _unitService.SwitchMode;
            _levelService.OnChangingGameMode += _constructionService.SwitchMode;
        }

        private void OnDestroy()
        {
            _input.OnPressSpace -= _levelService.OnSpacePressed;
            _levelService.OnCreatingWaves -= _unitService.CreateWave;
            _levelService.OnChangingGameMode -= _unitService.SwitchMode;
            _levelService.OnChangingGameMode -= _constructionService.SwitchMode;
        }
    }
}