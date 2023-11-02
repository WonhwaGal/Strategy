using System.Collections.Generic;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ConstructionPrefabs", menuName = "Construction/ConstructionPrefabs")]
    public class ConstructionPrefabs : PrefabSO<ConstructionView>
    {
        public List<PrefabData> Prefabs => _prefabs;
    }
}