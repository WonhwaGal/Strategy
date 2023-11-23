using Code.Pools;
using Code.Strategy;
using UnityEngine;
using static UnitSettingList;

namespace Code.Units
{
    public class UnitCreator
    {
        private readonly UnitMultiPool _multiPool;

        public UnitCreator(UnitMultiPool pool) => _multiPool = pool;

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
                new PlayerPresenter(view, model, strategy),
                PrefabType.Enemy =>
                new EnemyPresenter(view, model, strategy),
                _ => new AllyPresenter(view, model, strategy)
            };
        }

        internal void Despawn(PrefabType prefabType, UnitView view)
        {
            if (view != null)
                _multiPool.Despawn(prefabType, view);
        }
    }
}