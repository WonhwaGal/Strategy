using Code.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(UnitPrefabs), menuName = "Construction/UnitPrefabs")]
    public class UnitPrefabs : ScriptableObject
    {
        public List<PrefabData> UnitsPrefabs;

        [System.Serializable]
        public class PrefabData
        {
            public string Name;
            public PrefabType Type;
            public UnitView Prefab;
        }
    }
}
