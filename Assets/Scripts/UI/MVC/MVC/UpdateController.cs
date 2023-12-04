using System;

namespace Code.MVC
{
    public abstract class UpdateController<V, M> : Controller<V, M>, IUpdatableController
            where V : class, IUiView
            where M : class, IUiModel, new()
    {
        public string Tag => GetType().Name;

        void IUpdatableController.UpdateController(string tag)
        {
            if (string.IsNullOrEmpty(tag) || tag.Equals(Tag))
                return;
            UpdateView();
        }
    }
}