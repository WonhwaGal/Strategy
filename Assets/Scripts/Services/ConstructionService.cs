using System.Collections.Generic;
using Code.Factories;
using Code.ScriptableObjects;

namespace Code.Construction
{
    public sealed class ConstructionService : IService
    {
        private readonly ConstructionSO ConstructionSO;
        private readonly Pool<ConstructionView> Pool;

        public ConstructionService(ConstructionSO constructionSO, ConstructionPrefabs prefabs)
        {
            ConstructionSO = constructionSO;
            Pool = new ConstructionPool<ConstructionView>(prefabs);
        }

        public void StartLevel(int lvlNumber)
        {
            List<SingleBuildingData> buildings;
            for(int i = 0; i < ConstructionSO.ConstructionLevels.Count; i++)
            {
                if(ConstructionSO.ConstructionLevels[i].Level == lvlNumber)
                {
                    buildings = ConstructionSO.ConstructionLevels[i].BuildingList;
                    CreatePresenters(buildings);
                    return;
                }
            }
        }

        private void CreatePresenters(List<SingleBuildingData> buildings)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                ConstructionView buildingView = Pool.Spawn(buildings[i]);
                var model = new ConstructionModel(buildings[i].QueueOrder, buildings[i].Defense);
                var strategy = AssignStrategy(buildings[i].PrefabType);

                var presenter = new ConstructionPresenter(buildingView, model, strategy);
                presenter.OnDestroyObj += DestroyBuilding;
            }
        }

        private IConstructionStrategy AssignStrategy(PrefabType type) => new TestStrategy();

        private void DestroyBuilding(ConstructionPresenter presenter)
        {
            presenter.Dispose();
            presenter.OnDestroyObj -= DestroyBuilding;
        }
    }
}