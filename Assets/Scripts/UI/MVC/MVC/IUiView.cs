using System;

namespace Code.MVC
{
    public interface IUiView
    {
        event Action<bool> OnDisableView;
    }
}