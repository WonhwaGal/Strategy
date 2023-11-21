using System.Collections.Generic;
using Code.Pools;
using UnityEngine;

namespace Code.ScriptableObjects
{
    public abstract class UIPrefabSO<T> : ScriptableObject where T : class, ISpawnableType
    {
        [SerializeField] protected List<UIData> _prefabs;

        public virtual T FindPrefab(UIType type)
        {
            var data = _prefabs.Find(x => x.Type == type);
            if (data != null)
                return data.Prefab;
            else
                Debug.LogError($"{name} : {type} is not found");

            return null;
        }

        [System.Serializable]
        public class UIData
        {
            public string Name;
            public UIType Type;
            public T Prefab;
        }
    }
}