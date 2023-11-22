using Code.Pools;
using Code.Strategy;
using Code.UI;
using static UnitSettingList;

namespace Code.Units
{
    public class UnitCreator
    {
        private readonly UnitMultiPool _multiPool;
        private readonly UIService _uiService;

        public UnitCreator(UnitMultiPool pool)
        {
            _multiPool = pool;
            _uiService = ServiceLocator.Container.RequestFor<UIService>();
        }

        public UnitPresenter CreatePresenter(PrefabType type, UnitSettings settings)
        {
            var view = _multiPool.Spawn(type);
            var model = new UnitModel(settings, view.transform);
            var strategy = (IUnitStrategy)StrategyHandler.GetStrategy(type);
            var presenter = GetByType(type, view, model, strategy);
            strategy.Init(presenter);
            return presenter;
        }

        private UnitPresenter GetByType(PrefabType unitType, UnitView view, UnitModel model, IUnitStrategy strategy)
        {
            return unitType switch
            {
                PrefabType.Player => 
                new PlayerPresenter(view, model, strategy, _uiService.SpawnHPBar(UIType.PlayerHP)),
                PrefabType.Enemy => 
                new EnemyPresenter(view, model, strategy, _uiService.SpawnHPBar(UIType.EnemyHP)),
                _ => new AllyPresenter(view, model, strategy, _uiService.SpawnHPBar(UIType.PlayerHP))
            };
        }

        internal void Despawn(PrefabType prefabType, UnitView view)
        {
            if (view != null)
                _multiPool.Despawn(prefabType, view);
        }
    }
}