using UnityEngine;

namespace Code.Combat
{
    [System.Serializable]
    public class EnemySpawnData
    {
        [Range(1, 5)] public int WaveTurn;
        public PrefabType EnemyType;
        public int EnemyQuantity;
        public Transform SpawnCenter;
        public float SpawnRadius;
    }
}