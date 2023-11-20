using Code.Construction;
using Code.ScriptableObjects;
using Code.Units;

namespace Code.Pools
{
    public class RuinMultiPool : MultiPool<PrefabType, RuinView>
    {
        private readonly RuinList _prefabs;

        public RuinMultiPool(RuinList prefabs) => _prefabs = prefabs;

        protected override RuinView GetPrefab(PrefabType type) => _prefabs.FindPrefab(type);

        public override void OnSpawned(RuinView result, ISpawnableType data)
        {
            IUnitView buildingData = data as IUnitView;
            result.transform.position = buildingData.GameObject.transform.position;
            result.transform.rotation = buildingData.GameObject.transform.rotation;
        }
    }
}