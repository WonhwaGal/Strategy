using Code.Pools;
using Code.ScriptableObjects;
using Code.Strategy;
using System.Collections.Generic;

namespace Code.Construction
{
    public sealed class PresenterRegistry
    {
        private readonly Dictionary<PrefabType, ConstructionPresenter> _presenters = new ();
        //private readonly Pool<ConstructionView> _pool;
        private readonly MultiPool<ConstructionView> _multiPool;
        private readonly StrategyHandler _strategyHandler;

        public PresenterRegistry(MultiPool<ConstructionView> pool)
        {
            _multiPool = pool;
            _strategyHandler = ServiceLocator.Container.RequestFor<StrategyHandler>();
        }

        public ConstructionPresenter CreatePresenter(SingleBuildingData buildingData)
        {
            if (_presenters.ContainsKey(buildingData.PrefabType))
                return ClonePresenter(buildingData);
            else
                return AddPresenter(buildingData);
        }

        public ConstructionPresenter ClonePresenter(SingleBuildingData buildingData) 
            => _presenters[buildingData.PrefabType].Clone(
                _multiPool.Spawn(buildingData.PrefabType), buildingData.UniqueInfo);

        public ConstructionPresenter AddPresenter(SingleBuildingData buildingData)
        {
            var buildingView = _multiPool.Spawn(buildingData.PrefabType);
            _multiPool.OnSpawned(buildingView, buildingData);
            var model = new ConstructionModel(buildingData);
            var strategy = 
                (IConstructionStrategy)_strategyHandler.GetStrategy(buildingData.PrefabType);

            var presenter = new ConstructionPresenter(buildingView, model, strategy);
            _presenters.Add(buildingData.PrefabType, presenter);
            return presenter;
        }
    }
}