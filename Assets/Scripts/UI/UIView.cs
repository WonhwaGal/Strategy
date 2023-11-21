using Code.Pools;
using UnityEngine;

namespace Code.UI
{
    public class UIView : MonoBehaviour, ISpawnableType
    {
        [SerializeField] private PrefabType _prefabType;
        [SerializeField] private UIType _uiType;

        public PrefabType PrefabType => _prefabType;
        public UIType UIType  => _uiType;
    }
}