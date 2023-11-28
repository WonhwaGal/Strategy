namespace Code.MVC
{
    public abstract class Controller<V, M> : IController<V, M>
where V : class, IUiView
where M : class, IUiModel, new()
    {
        public Controller()
        {
            Model = new M();
        }

        public V View { get; private set; }

        public M Model { get; }

        public abstract void UpdateView();

        void IController.AddView<T>(T view)
        {
            if (View != null)
                return;
            View = view as V;
            View.OnDisableView += Hide;
        }

        protected abstract void Show();
        protected abstract void Hide(bool isDestroyed);
    }
}