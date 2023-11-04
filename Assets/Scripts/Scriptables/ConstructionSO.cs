using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(ConstructionSO), menuName = "Construction/ConstructionSO")]
    public class ConstructionSO : ScriptableObject
    {
        [SerializeField] private List<ConstructionLevel> _constructionLevels;

        public List<SingleBuildingData> FindBuildingsOfLevel(int lvlNumber)
        {
            var lvl = _constructionLevels.Find(x => x.Level == lvlNumber);
            if (lvl != null)
                return lvl.BuildingList;
            else
                Debug.LogError($"{name} : level #{lvl.Level} is not found");
            
            return null;
        }

        [Serializable]
        public class ConstructionLevel
        {
            public string Name;
            public int Level;
            public List<SingleBuildingData> BuildingList = new();
        }
    }
}