using System.Collections.Generic;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(ConstructionPrefabs), menuName = "Construction/PrefabSO")]
    public class ConstructionPrefabs : ScriptableObject
    {
        public List<PrefabData> Prefabs;

        [System.Serializable]
        public class PrefabData
        {
            public string Name;
            public PrefabType Type;
            public ConstructionView Prefab;
        }
    }
}