using UnityEngine;
using Code.Pools;

namespace Code.Construction
{
    public class RuinView : MonoBehaviour, ISpawnableType
    {
        [SerializeField] private PrefabType _prefabType;
        public PrefabType PrefabType => _prefabType;
    }
}