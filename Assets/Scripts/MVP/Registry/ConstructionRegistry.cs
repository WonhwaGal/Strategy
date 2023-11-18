using Code.Pools;
using Code.ScriptableObjects;
using Code.Strategy;

namespace Code.Construction
{
    public sealed class ConstructionRegistry
    {
        private readonly MultiPool<PrefabType, ConstructionView> _multiPool;
        private readonly StrategyHandler _strategyHandler;

        public ConstructionRegistry(MultiPool<PrefabType, ConstructionView> pool)
        {
            _multiPool = pool;
            _strategyHandler = ServiceLocator.Container.RequestFor<StrategyHandler>();
        }

        public ConstructionPresenter CreatePresenter(SingleBuildingData buildingData)
        {
            var buildingView = _multiPool.Spawn(buildingData.PrefabType);
            _multiPool.OnSpawned(buildingView, buildingData);
            var model = new ConstructionModel(buildingData);
            var strategy = 
                (IConstructionStrategy)_strategyHandler.GetStrategy(buildingData.PrefabType);

            var presenter = new ConstructionPresenter(buildingView, model, strategy);
            strategy.Init(presenter);

            return presenter;
        }
    }
}