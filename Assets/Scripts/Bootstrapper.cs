using UnityEngine;
using UnityEngine.SceneManagement;
using Code.Input;
using Code.Strategy;
using Code.Construction;
using Code.ScriptableObjects;
using Code.Combat;

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

        private const string GameScene = "GameScene";

        private IInputService _input;
        private UnitService _unitService;
        private LevelService _levelService;
        private CombatService _combatService;
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
            _combatService = ServiceLocator.Container.RegisterAndAssign(new CombatService(_weaponList));
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
            _unitService.OnRegisterForCombat += _combatService.RegisterParticipants;
            _constructionService.OnRegisterForCombat += _combatService.RegisterParticipants;
            _combatService.OnCombatEnded += _levelService.SwitchToDay;
        }

        private void OnDestroy()
        {
            _input.OnPressSpace -= _levelService.OnSpacePressed;
            _levelService.OnCreatingWaves -= _unitService.CreateWave;
            _levelService.OnChangingGameMode -= _unitService.SwitchMode;
            _levelService.OnChangingGameMode -= _constructionService.SwitchMode;
            _unitService.OnRegisterForCombat -= _combatService.RegisterParticipants;
            _constructionService.OnRegisterForCombat -= _combatService.RegisterParticipants;
            _combatService.OnCombatEnded -= _levelService.SwitchToDay;
        }
    }
}