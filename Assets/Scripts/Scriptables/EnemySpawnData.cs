using UnityEngine;

public partial class WaveSO
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public PrefabType EnemyType;
        public int EnemyQuantity;
        public Transform SpawnCenter;
        public float SpawnRadius;
    }
}