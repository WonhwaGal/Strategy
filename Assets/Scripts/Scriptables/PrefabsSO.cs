using Code.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Code.ScriptableObjects
{
    public abstract class PrefabSO<T> : ScriptableObject where T : class, IUnitView
    {
        [SerializeField] protected List<PrefabData> _prefabs;

        public virtual T FindPrefab(PrefabType type)
        {
            var data = _prefabs.Find(x => x.Type == type);
            if (data != null)
                return data.Prefab;
            else
                Debug.LogError($"{name} : {type} is not found");

            return null;
        }

        [System.Serializable]
        public class PrefabData
        {
            public string Name;
            public PrefabType Type;
            public T Prefab;
        }
    }
}