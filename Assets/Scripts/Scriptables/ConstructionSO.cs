using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(ConstructionSO), menuName = "Construction/ConstructionSO")]
    public class ConstructionSO : ScriptableObject
    {
        public List<ConstructionLevel> ConstructionLevels = new();

        [Serializable]
        public class ConstructionLevel
        {
            public string Name;
            public int Level;
            public List<SingleBuildingData> BuildingList = new();
        }
    }
}