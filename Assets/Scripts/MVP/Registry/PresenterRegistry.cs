using Code.Factories;
using Code.ScriptableObjects;
using Code.Strategy;
using System.Collections.Generic;

namespace Code.Construction
{
    public sealed class PresenterRegistry
    {
        private readonly Dictionary<PrefabType, ConstructionPresenter> _presenters;
        private readonly Pool<ConstructionView> _pool;
        private readonly StrategyHandler _strategyHandler;
        public PresenterRegistry(Pool<ConstructionView> pool)
        {
            _pool = pool;
            _presenters = new();
            _strategyHandler = ServiceLocator.Container.RequestFor<StrategyHandler>();
        }

        public ConstructionPresenter CreatePresenter(SingleBuildingData buildingData)
        {
            if (_presenters.ContainsKey(buildingData.CommonInfo.PrefabType))
                return ClonePresenter(buildingData);
            else
                return AddPresenter(buildingData);
        }

        public ConstructionPresenter ClonePresenter(SingleBuildingData buildingData) 
            => _presenters[buildingData.CommonInfo.PrefabType].Clone(_pool.Spawn(buildingData), buildingData.UniqueInfo);

        public ConstructionPresenter AddPresenter(SingleBuildingData buildingData)
        {
            var buildingView = _pool.Spawn(buildingData);
            var model = new ConstructionModel(buildingData);
            var strategy = 
                (IConstructionStrategy)_strategyHandler.GetStrategy(buildingData.CommonInfo.PrefabType);

            var presenter = new ConstructionPresenter(buildingView, model, strategy);
            _presenters.Add(buildingData.CommonInfo.PrefabType, presenter);
            return presenter;
        }
    }
}