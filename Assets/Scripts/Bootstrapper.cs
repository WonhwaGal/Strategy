using UnityEngine;
using UnityEngine.SceneManagement;
using Code.Input;
using Code.Strategy;
using Code.Construction;
using Code.ScriptableObjects;
using Code.Combat;
using Code.Units;
using Code.UI;

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
        [SerializeField] private UnsortedUIList _unsortedUIList;
        [SerializeField] private HPBarList _hpBarList;

        private const string GameScene = "GameScene";

        private UIService _uiService;
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
            ServiceLocator.Container.Register<IInputService>(new KeyboardInput());
            ServiceLocator.Container.Register(new StrategyHandler());
            ServiceLocator.Container.Register(new CombatService(_weaponList));
            _uiService = ServiceLocator.Container.RegisterAndAssign(new UIService(_unsortedUIList, _hpBarList));
            _unitService = ServiceLocator.Container.RegisterAndAssign(new UnitService(_unitSetList, _unitPrefabs));
            _constructionService = ServiceLocator.Container.
                RegisterAndAssign(new ConstructionService(_constructionSO, _buildingPrefabs));
            _levelService = ServiceLocator.Container.RegisterAndAssign(new LevelService(_wavesList));
            Subsribe();
        }

        private void Subsribe()
        {
            _levelService.OnCreatingWaves += _unitService.CreateWave;
            _levelService.OnChangingGameMode += _unitService.SwitchMode;
            _levelService.OnChangingGameMode += _constructionService.SwitchMode;
            _constructionService.OnNotifyConnections += _uiService.ShowPanel;
            _uiService.OnChooseUpgrade += _constructionService.Upgrade;
        }

        private void OnDestroy()
        {
            _levelService.OnCreatingWaves -= _unitService.CreateWave;
            _levelService.OnChangingGameMode -= _unitService.SwitchMode;
            _levelService.OnChangingGameMode -= _constructionService.SwitchMode;
            _constructionService.OnNotifyConnections -= _uiService.ShowPanel;
            _uiService.OnChooseUpgrade -= _constructionService.Upgrade;
            _levelService.Dispose();
        }
    }
}