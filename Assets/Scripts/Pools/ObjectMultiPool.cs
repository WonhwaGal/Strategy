using Code.Combat;

namespace Code.Pools
{
    public sealed class ObjectMultiPool : MultiPool<PrefabType, WeaponView>
    {
        private readonly WeaponList _prefabs;

        public ObjectMultiPool(WeaponList prefabs) => _prefabs = prefabs;

        public int GetDamage(PrefabType type) => GetPrefab(type).Damage;

        protected override WeaponView GetPrefab(PrefabType type) => _prefabs.FindPrefab(type);
    }
}