namespace Code.MVC
{
    public interface IController
    {
        void AddView<T>(T view) where T : class, IUiView;
    }

    public interface IController<V, M> : IController
        where V : class, IUiView
        where M : class, IUiModel
    {
        V View { get; }
        M Model { get; }
        void UpdateView();
    }
}