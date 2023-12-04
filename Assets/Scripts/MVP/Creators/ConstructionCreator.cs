using Code.Pools;
using Code.ScriptableObjects;
using Code.Strategy;

namespace Code.Construction
{
    public sealed class ConstructionCreator
    {
        private readonly ConstructionMultiPool _multiPool;

        public ConstructionCreator(ConstructionMultiPool pool) => _multiPool = pool;

        public ConstructionPresenter CreatePresenter(SingleBuildingData buildingData)
        {
            var buildingView = _multiPool.Spawn(buildingData.PrefabType);
            _multiPool.OnSpawned(buildingView, buildingData);
            var model = new ConstructionModel(buildingData);
            var strategy =
                (IConstructionStrategy)StrategyHandler.GetStrategy(buildingData.PrefabType);

            ConstructionPresenter presenter;
            if (buildingData.PrefabType == PrefabType.Barracks)
                presenter = new SpawnConstructionPresenter(buildingView, model, strategy);
            else
                presenter = new ConstructionPresenter(buildingView, model, strategy);
            strategy.Init(presenter);
            return presenter;
        }
    }
}