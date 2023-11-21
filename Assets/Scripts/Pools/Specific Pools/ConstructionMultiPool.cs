using Code.Construction;
using Code.ScriptableObjects;

namespace Code.Pools
{
    public sealed class ConstructionMultiPool : MultiPool<PrefabType, ConstructionView>
    {
        private readonly ConstructionPrefabs _prefabs;

        public ConstructionMultiPool(ConstructionPrefabs prefabs) => _prefabs = prefabs;

        protected override ConstructionView GetPrefab(PrefabType type) => _prefabs.FindPrefab(type);

        public override void OnSpawned(ConstructionView result, ISpawnableType data)
        {
            ISpawnBuilding buildingData = data as ISpawnBuilding;
            result.transform.position = buildingData.Transform.position;
            result.transform.rotation = buildingData.Transform.rotation;
        }
    }
}