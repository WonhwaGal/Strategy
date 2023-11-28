using UnityEngine;

namespace Code.UI
{
    public class FollowUIView : UIView
    {
        [SerializeField] private float _verticalShift;

        protected Transform _owner;
        private Camera _camera;
        protected bool _isSetUp;

        private void OnEnable() => _camera = Camera.main;

        private void LateUpdate()
        {
            if (!_isSetUp)
                return;

            FollowOwner();
        }

        public void Despawn()
            => ServiceLocator.Container.RequestFor<FollowUIPool>().Despawn(UIType, this);

        private void FollowOwner()
        {
            transform.position = _camera.WorldToScreenPoint(_owner.position);
            transform.position += new Vector3(0, _verticalShift, 0);
        }
    }
}