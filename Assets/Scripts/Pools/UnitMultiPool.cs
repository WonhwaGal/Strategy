using Code.ScriptableObjects;
using Code.Units;

namespace Code.Pools
{
    public sealed class UnitMultiPool : MultiPool<PrefabType, UnitView>
    {
        private readonly UnitPrefabs _prefabs;

        public UnitMultiPool(UnitPrefabs prefabs) => _prefabs = prefabs;

        protected override UnitView GetPrefab(PrefabType type) => _prefabs.FindPrefab(type);
    }
}