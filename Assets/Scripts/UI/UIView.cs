using System;
using UnityEngine;
using Code.Pools;
using Code.MVC;

namespace Code.UI
{
    public class UIView : MonoBehaviour, IUiView, ISpawnableType
    {
        [SerializeField] private PrefabType _prefabType;
        [SerializeField] private UIType _uiType;

        public PrefabType PrefabType => _prefabType;
        public UIType UIType => _uiType;


        public event Action<bool> OnDisableView;

        protected virtual void OnViewDestroy() { }

        private void OnDestroy()
        {
            OnDisableView?.Invoke(true);
            OnDisableView = null;
            OnViewDestroy();
        }
    }
}