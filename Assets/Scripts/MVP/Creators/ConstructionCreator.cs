using Code.Pools;
using Code.ScriptableObjects;
using Code.Strategy;
using Code.UI;
using UnityEngine;

namespace Code.Construction
{
    public sealed class ConstructionCreator
    {
        private readonly ConstructionMultiPool _multiPool;
        private readonly UIService _uiService;

        public ConstructionCreator(ConstructionMultiPool pool)
        {
            _multiPool = pool;
            _uiService = ServiceLocator.Container.RequestFor<UIService>();
        }

        public ConstructionPresenter CreatePresenter(SingleBuildingData buildingData)
        {
            var buildingView = _multiPool.Spawn(buildingData.PrefabType);
            _multiPool.OnSpawned(buildingView, buildingData);
            var model = new ConstructionModel(buildingData);
            var strategy =
                (IConstructionStrategy)StrategyHandler.GetStrategy(buildingData.PrefabType);

            ConstructionPresenter presenter;
            if (buildingData.PrefabType == PrefabType.Barracks)
                presenter = new SpawnConstructionPresenter(buildingView, model, strategy, GetBarByType(model.PrefabType));
            else
                presenter = new ConstructionPresenter(buildingView, model, strategy, GetBarByType(model.PrefabType));
            strategy.Init(presenter);

            return presenter;
        }

        private HPBar GetBarByType(PrefabType constructionType)
        {
            return constructionType switch
            {
                PrefabType.Castle => _uiService.SpawnHPBar(UIType.CastleHP),
                _ => _uiService.SpawnHPBar(UIType.BuildingHP),
            };
        }
    }
}