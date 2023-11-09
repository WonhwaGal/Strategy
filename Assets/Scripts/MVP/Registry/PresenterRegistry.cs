using Code.Pools;
using Code.ScriptableObjects;
using Code.Strategy;
using System.Collections.Generic;

namespace Code.Construction
{
    public sealed class PresenterRegistry
    {
        private readonly Dictionary<PrefabType, ConstructionPresenter> _presenters = new ();
        private readonly Pool<ConstructionView> _pool;
        private readonly StrategyHandler _strategyHandler;

        public PresenterRegistry(Pool<ConstructionView> pool)
        {
            _pool = pool;
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
            => _presenters[buildingData.PrefabType].Clone(_pool.Spawn(buildingData), buildingData.UniqueInfo);

        public ConstructionPresenter AddPresenter(SingleBuildingData buildingData)
        {
            var buildingView = _pool.Spawn(buildingData);
            var model = new ConstructionModel(buildingData);
            var strategy = 
                (IConstructionStrategy)_strategyHandler.GetStrategy(buildingData.PrefabType);

            var presenter = new ConstructionPresenter(buildingView, model, strategy);
            _presenters.Add(buildingData.PrefabType, presenter);
            return presenter;
        }
    }
}