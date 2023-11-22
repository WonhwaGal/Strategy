using UnityEngine;

namespace Code.Construction
{
    public class SpawnConstructionView : ConstructionView
    {
        [SerializeField] private Transform[] _allySpawnPoints;

        public Transform[] AllySpawnPoints => _allySpawnPoints;
    }
}